namespace ShippingOrder.Domain.Events;

public sealed record ShippingOrderClosedDomainEvent(Models.ShippingOrder Order) : IDomainEvent;
