namespace ShippingOrder.Domain.Events;

public sealed record ShippingOrderCreatedDomainEvent(Models.ShippingOrder Order) : IDomainEvent;
