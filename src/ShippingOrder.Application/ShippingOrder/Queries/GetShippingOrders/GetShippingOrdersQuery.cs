using ERP.Shared.CQRS;
using ERP.Shared.Pagination;

namespace ShippingOrder.Application.ShippingOrder.Queries.GetShippingOrders;

public record GetShippingOrdersQuery(PaginationRequest PaginationRequest,
    DateTime? StartDate = null,
    DateTime? EndDate = null,
    string? State = null)

    : IQuery<GetShippingOrdersResults>;

public record GetShippingOrdersResults(PaginatedResult<ShippingOrderDto> Orders);