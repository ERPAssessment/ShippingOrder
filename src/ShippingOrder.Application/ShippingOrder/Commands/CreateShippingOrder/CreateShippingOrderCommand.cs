namespace ShippingOrder.Application.ShippingOrder.Commands.CreateShippingOrder;

public record CreateShippingOrderCommand(CreateShippingOrderDto Order)
    : ICommand<CreateShippingOrderResult>;

public record CreateShippingOrderResult(Guid Id);
