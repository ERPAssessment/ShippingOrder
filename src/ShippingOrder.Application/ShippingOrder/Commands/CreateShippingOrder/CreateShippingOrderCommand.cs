namespace ShippingOrder.Application.ShippingOrder.Commands.CreateShippingOrder;

public record CreateShippingOrderCommand(CreateShippingOrderDto SHOrder)
    : ICommand<CreateShippingOrderResult>;

public record CreateShippingOrderResult(Guid SHOrderId);
