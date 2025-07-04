namespace ShippingOrder.Infrastructure.Workers;

[DisallowConcurrentExecution]
public class ProcessOutboxMessagesJob
(IApplicationDbContext _dbContext,
      IMediator _mediator,
      ILogger<ProcessOutboxMessagesJob> _logger) : IJob
{
  private const int BatchSize = 20;
  private const int MaxRetryAttempts = 3;

  public async Task Execute(IJobExecutionContext context)
  {
    using var scope = _logger.BeginScope(new { JobId = context.FireInstanceId });
    _logger.LogInformation("Starting outbox message processing job at {Timestamp}", DateTime.UtcNow);

    try
    {
      var messages = await GetUnprocessedMessagesAsync(context.CancellationToken);

      if (!messages.Any())
      {
        _logger.LogInformation("No unprocessed messages found");
        return;
      }

      _logger.LogInformation("Found {MessageCount} unprocessed messages", messages.Count);

      await ProcessMessagesAsync(messages, context.CancellationToken);

      await SaveChangesAsync(context.CancellationToken);

      _logger.LogInformation("Successfully processed {MessageCount} messages", messages.Count);
    }
    catch (Exception ex)
    {
      _logger.LogError(ex, "Failed to process outbox messages");
      throw;
    }
  }

  private async Task<List<OutboxMessage>> GetUnprocessedMessagesAsync(CancellationToken cancellationToken)
  {
    _logger.LogDebug("Querying unprocessed outbox messages");

    return await _dbContext
        .OutboxMessages
        .Where(m => m.ProcessedOnUtc == null)
        .OrderBy(m => m.OccurredOnUtc)
        .Take(BatchSize)
        .ToListAsync(cancellationToken);
  }

  private async Task ProcessMessagesAsync(List<OutboxMessage> messages, CancellationToken cancellationToken)
  {
    foreach (var outboxMessage in messages)
    {
      await ProcessSingleMessageAsync(outboxMessage, cancellationToken);
    }
  }

  private async Task ProcessSingleMessageAsync(OutboxMessage outboxMessage, CancellationToken cancellationToken)
  {
    using var messageScope = _logger.BeginScope(new { MessageId = outboxMessage.Id });
    _logger.LogDebug("Processing outbox message");

    for (int attempt = 1; attempt <= MaxRetryAttempts; attempt++)
    {
      try
      {
        var domainEvent = DeserializeDomainEvent(outboxMessage);

        if (domainEvent == null)
        {
          _logger.LogWarning("Failed to deserialize domain event from message content");
          outboxMessage.Error = "Failed to deserialize domain event";
          outboxMessage.ProcessedOnUtc = DateTime.UtcNow;
          return;
        }

        _logger.LogInformation("Publishing domain event {EventType}", domainEvent.GetType().Name);

        await _mediator.Publish(domainEvent, cancellationToken);

        outboxMessage.ProcessedOnUtc = DateTime.UtcNow;
        outboxMessage.Error = null;

        _logger.LogDebug("Successfully processed message");
        return;
      }
      catch (Exception ex)
      {
        _logger.LogError(ex, "Error processing message on attempt {Attempt}/{MaxAttempts}", attempt, MaxRetryAttempts);

        outboxMessage.Error = ex.Message;

        if (attempt == MaxRetryAttempts)
        {
          _logger.LogError("Max retry attempts reached for message");
          outboxMessage.ProcessedOnUtc = DateTime.UtcNow;
          return;
        }

        await Task.Delay(CalculateBackoff(attempt), cancellationToken);
      }
    }
  }

  private INotification? DeserializeDomainEvent(OutboxMessage outboxMessage)
  {
    try
    {
      var domainAssembly = typeof(ShippingOrderCreatedEvent).Assembly;

      return JsonConvert.DeserializeObject<INotification>(
          outboxMessage.Content,
          new JsonSerializerSettings
          {
            TypeNameHandling = TypeNameHandling.All,
            ConstructorHandling = ConstructorHandling.AllowNonPublicDefaultConstructor,
            Error = (sender, args) =>
            {
              _logger.LogError(args.ErrorContext.Error, "Deserialization error");
              args.ErrorContext.Handled = true;
            }
          });
    }
    catch (Exception ex)
    {
      _logger.LogError(ex, "Failed to deserialize domain event");
      return null;
    }
  }

  private async Task SaveChangesAsync(CancellationToken cancellationToken)
  {
    _logger.LogDebug("Saving changes to database");

    try
    {
      await _dbContext.SaveChangesAsync(cancellationToken);
      _logger.LogDebug("Changes saved successfully");
    }
    catch (Exception ex)
    {
      _logger.LogError(ex, "Failed to save changes to database");
      throw;
    }
  }

  private static TimeSpan CalculateBackoff(int attempt)
  {
    // Exponential backoff with jitter
    int delayMs = (int)(Math.Pow(2, attempt) * 100 + Random.Shared.Next(0, 100));
    return TimeSpan.FromMilliseconds(delayMs);
  }

}