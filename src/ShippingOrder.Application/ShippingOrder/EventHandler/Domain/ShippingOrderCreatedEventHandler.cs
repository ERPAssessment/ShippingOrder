using ERP.Shared.Events;
using MassTransit;
using MediatR;
using Microsoft.Extensions.Logging;
using ShippingOrder.Domain.Events;

namespace ShippingOrder.Application.ShippingOrder.EventHandler.Domain;

public class ShippingOrderCreatedEventHandler
 (IPublishEndpoint publishEndpoint,
   ILogger<ShippingOrderCreatedEventHandler> logger)
    : INotificationHandler<ShippingOrderCreatedEvent>
{
  public async Task Handle(ShippingOrderCreatedEvent domainEvent, CancellationToken cancellationToken)
  {
    logger.LogInformation("Domain Event handled: {DomainEvent}", domainEvent.GetType().Name);

    var createdOrderEvent = new SHOCreatedEvent() { PurchaseOrderNumber = domainEvent.Order.PONumber.Value };
    await publishEndpoint.Publish(createdOrderEvent, cancellationToken);
  }
}
