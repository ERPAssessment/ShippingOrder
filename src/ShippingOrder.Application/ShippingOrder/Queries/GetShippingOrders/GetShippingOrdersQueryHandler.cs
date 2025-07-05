using ERP.Shared.Pagination;
using ShippingOrder.Application.Extenstions;
using ShippingOrder.Domain.Enums;
using ShippingOrder.Domain.Specifications.Shared;
using ShippingOrder.Domain.Specifications.ShippingOrderSpecs;
using Models = ShippingOrder.Domain.Models;
namespace ShippingOrder.Application.ShippingOrder.Queries.GetShippingOrders;

internal class GetShippingOrdersQueryHandler(IReadShippingOrderRepository shippingOrderRepository)
    : IQueryHandler<GetShippingOrdersQuery, GetShippingOrdersResults>
{
  public async Task<GetShippingOrdersResults> Handle(GetShippingOrdersQuery query, CancellationToken cancellationToken)
  {
    var pageIndex = query.PaginationRequest.PageIndex;
    var pageSize = query.PaginationRequest.PageSize;
    var specification = BuildSpecification(query);

    return await GetShippingOrdersData(specification, pageIndex, pageSize, cancellationToken);
  }

  private async Task<GetShippingOrdersResults> GetShippingOrdersData(
      Specification<Models.ShippingOrder>? specification,
      int pageIndex,
      int pageSize,
      CancellationToken cancellationToken)
  {
    IEnumerable<Models.ShippingOrder> orders;
    long totalCount;

    if (specification != null)
    {
      orders = await shippingOrderRepository.FindAsync(specification, pageIndex, pageSize, cancellationToken);
      totalCount = orders.Count();
    }
    else
    {
      totalCount = await shippingOrderRepository.GetTotalCountAsync(cancellationToken);
      orders = await shippingOrderRepository.GetPagedShippingOrders(pageIndex, pageSize, cancellationToken);
    }

    var dtoList = orders.ToShipppingOrdersDtoList();
    var paginatedResult = new PaginatedResult<ShippingOrderDto>(pageIndex, pageSize, totalCount, dtoList);

    return new GetShippingOrdersResults(paginatedResult);
  }


  private Specification<Models.ShippingOrder>? BuildSpecification(GetShippingOrdersQuery query)
  {
    var specifications = new List<Specification<Models.ShippingOrder>>();

    if (query.StartDate.HasValue || query.EndDate.HasValue)
    {
      specifications.Add(new ShippingOrderByDateRangeSpecification(
          query.StartDate ?? DateTime.MinValue,
          query.EndDate ?? DateTime.MaxValue));
    }

    if (!string.IsNullOrEmpty(query.State)
      && Enum.TryParse<ShippingOrderState>(query.State, true, out var stateEnum))
    {
      specifications.Add(new ShippingOrderByStateSpecification(stateEnum));
    }

    if (specifications.Count == 0)
      return null;

    if (specifications.Count == 1)
      return specifications[0];

    var combinedSpec = specifications[0];
    for (int i = 1; i < specifications.Count; i++)
    {
      combinedSpec = combinedSpec.And(specifications[i]);
    }

    return combinedSpec;
  }
}