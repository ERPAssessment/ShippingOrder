using MediatR;
using Microsoft.EntityFrameworkCore.Diagnostics;
using ShippingOrder.Domain.Abstractions;

namespace ShippingOrder.Infrastructure.Data.Interceptors;

public class DispatchDomainEventsInterceptor(IMediator mediator)
    : SaveChangesInterceptor
{

  public override int SavedChanges(SaveChangesCompletedEventData eventData, int result)
  {
    DispatchDomainEvents(eventData.Context).GetAwaiter().GetResult();
    return result;
  }

  public override async ValueTask<int> SavedChangesAsync(SaveChangesCompletedEventData eventData,
                                                        int result,
                                                        CancellationToken cancellationToken = default)
  {
    await DispatchDomainEvents(eventData.Context);
    return result;
  }

  public async Task DispatchDomainEvents(DbContext? context)
  {
    if (context == null) return;

    var aggregates = context.ChangeTracker
        .Entries<IAggregate>()
        .Where(a => a.Entity.DomainEvents.Any())
        .Select(a => a.Entity);

    var domainEvents = aggregates
        .SelectMany(a => a.DomainEvents)
        .ToList();

    aggregates.ToList().ForEach(a => a.ClearDomainEvents());

    foreach (var domainEvent in domainEvents)
      await mediator.Publish(domainEvent);
  }
}
