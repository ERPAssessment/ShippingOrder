using ERP.Shared.Events;
using Microsoft.EntityFrameworkCore.Diagnostics;
using ShippingOrder.Domain.Abstractions;

namespace ShippingOrder.Infrastructure.Data.Interceptors;

public class DispatchDomainEventsInterceptor : SaveChangesInterceptor
{
  public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
  {
    DispatchDomainEvents(eventData.Context, default).GetAwaiter().GetResult();
    return base.SavingChanges(eventData, result);
  }

  public override async ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result, CancellationToken cancellationToken = default)
  {
    await DispatchDomainEvents(eventData.Context, cancellationToken);
    return await base.SavingChangesAsync(eventData, result, cancellationToken);
  }

  private async Task DispatchDomainEvents(DbContext? context, CancellationToken cancellationToken)
  {
    if (context is not ApplicationDbContext dbContext) return;

    var aggregateRootEntries = dbContext.ChangeTracker
        .Entries<IAggregate>()
        .Where(x => x.Entity.DomainEvents.Any())
        .Select(x => x.Entity)
        .ToList();

    if (!aggregateRootEntries.Any()) return;

    var outboxMessages = new List<OutboxMessage>();
    var currentTimestamp = DateTime.UtcNow;

    foreach (var aggregateRoot in aggregateRootEntries)
    {
      var domainEvents = aggregateRoot.DomainEvents.ToList();
      aggregateRoot.ClearDomainEvents();

      foreach (var domainEvent in domainEvents)
      {
        try
        {
          var serializedContent = await SerializeDomainEventAsync(domainEvent, cancellationToken);
          outboxMessages.Add(new OutboxMessage
          {
            Id = Guid.NewGuid(),
            OccurredOnUtc = currentTimestamp,
            Type = domainEvent.GetType().AssemblyQualifiedName ?? domainEvent.GetType().Name,
            Content = serializedContent
          });
        }
        catch (Exception ex)
        {
          outboxMessages.Add(new OutboxMessage
          {
            Id = Guid.NewGuid(),
            OccurredOnUtc = currentTimestamp,
            Type = domainEvent.GetType().AssemblyQualifiedName ?? domainEvent.GetType().Name,
            Error = ex.Message
          });
        }
      }
    }

    if (outboxMessages.Any())
    {
      await dbContext.OutboxMessages.AddRangeAsync(outboxMessages, cancellationToken);
    }
  }

  private static async Task<string> SerializeDomainEventAsync(IDomainEvent domainEvent, CancellationToken cancellationToken)
  {
    var settings = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All };
    var eventDto = MapToDto(domainEvent);
    return await Task.Run(() => JsonConvert.SerializeObject(eventDto, settings), cancellationToken);
  }

  public static object MapToDto(IDomainEvent domainEvent)
  {
    return domainEvent switch
    {
      ShippingOrderCreatedDomainEvent e => new SHOCreatedEvent() { PurchaseOrderNumber = e.Order.PONumber.Value },
      ShippingOrderClosedDomainEvent e => new SHOClosedEvent() { PurchaseOrderNumber = e.Order.PONumber.Value },
      _ => throw new NotSupportedException($"No DTO mapping defined for {domainEvent.GetType().Name}")
    };
  }
}