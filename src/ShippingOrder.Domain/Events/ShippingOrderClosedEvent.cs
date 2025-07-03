namespace ShippingOrder.Domain.Events;

public record ShippingOrderClosedEvent(Models.ShippingOrder Order) : IDomainEvent;
