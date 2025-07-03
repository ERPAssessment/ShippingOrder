using MassTransit;
using MediatR;
using Microsoft.Extensions.Logging;
using ShippingOrder.Domain.Events;

namespace ShippingOrder.Application.ShippingOrder.EventHandler.Domain;

public class ShippingOrderClosedEventHandler
 (IPublishEndpoint publishEndpoint,
   ILogger<ShippingOrderClosedEventHandler> logger)
    : INotificationHandler<ShippingOrderClosedEvent>
{
  public async Task Handle(ShippingOrderClosedEvent domainEvent, CancellationToken cancellationToken)
  {
    logger.LogInformation("Domain Event handled: {DomainEvent}", domainEvent.GetType().Name);

  }
}
