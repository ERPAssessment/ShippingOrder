using ERP.Shared.CQRS;

namespace ShippingOrder.Application.ShippingOrder.Queries.GetShippingOrderById;

public record GetShippingOrderByIdQuery(string Id)
    : IQuery<GetShippingOrderByIdResults>;

public record GetShippingOrderByIdResults(ShippingOrderDto Order);
