using ShippingOrder.Shared.Pagination;

namespace PurchasingOrder.Application.PurchaseOrders.Queries.GetPurchaseOrders;

public record GetShippingOrdersQuery(PaginationRequest PaginationRequest,
    DateTime? StartDate = null,
    DateTime? EndDate = null,
    string? State = null)

    : IQuery<GetShippingOrdersResults>;

public record GetShippingOrdersResults(PaginatedResult<ShippingOrderDto> Orders);