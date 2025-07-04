using ERP.Shared.Events;
using MassTransit;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ShippingOrder.Application.ShippingOrder.EventHandler.Domain;

public class ShippingOrderCreatedEventHandler
 (IPublishEndpoint publishEndpoint,
   ILogger<ShippingOrderCreatedEventHandler> logger)
    : INotificationHandler<SHOCreatedEvent>
{
  public async Task Handle(SHOCreatedEvent domainEvent, CancellationToken cancellationToken)
  {
    logger.LogInformation("Domain Event handled: {DomainEvent}", domainEvent.GetType().Name);

    await publishEndpoint.Publish(domainEvent, cancellationToken);
  }
}
