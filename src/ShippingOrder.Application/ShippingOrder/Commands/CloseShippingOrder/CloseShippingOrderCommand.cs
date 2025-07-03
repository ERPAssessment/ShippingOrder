namespace ShippingOrder.Application.ShippingOrder.Commands.CloseShippingOrder;

public record CloseShippingOrderCommand(string SHOrderId)
    : ICommand<CloseShippingOrderResult>;

public record CloseShippingOrderResult(bool Result);
