using ERP.Shared.CQRS;

namespace ShippingOrder.Application.ShippingOrder.Commands.CloseShippingOrder;

public record CloseShippingOrderCommand(string OrderId)
    : ICommand<CloseShippingOrderResult>;

public record CloseShippingOrderResult(bool Result);
