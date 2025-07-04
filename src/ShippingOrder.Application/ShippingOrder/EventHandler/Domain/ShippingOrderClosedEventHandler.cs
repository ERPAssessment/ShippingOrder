using ERP.Shared.Events;
using MassTransit;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ShippingOrder.Application.ShippingOrder.EventHandler.Domain;

public class ShippingOrderClosedEventHandler
 (IPublishEndpoint publishEndpoint,
   ILogger<ShippingOrderClosedEventHandler> logger)
    : INotificationHandler<SHOClosedEvent>
{
  public async Task Handle(SHOClosedEvent domainEvent, CancellationToken cancellationToken)
  {
    logger.LogInformation("Domain Event handled: {DomainEvent}", domainEvent.GetType().Name);
    await publishEndpoint.Publish(domainEvent, cancellationToken);
  }
}
