namespace ShippingOrder.Domain.Events;

public record ShippingOrderCreatedEvent(Models.ShippingOrder Order) : IDomainEvent;
