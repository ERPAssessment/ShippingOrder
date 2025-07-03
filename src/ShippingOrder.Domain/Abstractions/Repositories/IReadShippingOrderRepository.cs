using ShippingOrder.Domain.Specifications.Shared;
using ShippingOrder.Domain.ValueObjects;

namespace ShippingOrder.Domain.Abstractions.Repositories;

public interface IReadShippingOrderRepository
{
  Task<Models.ShippingOrder?> GetById(ShippingOrderId Id, CancellationToken cancellationToken);
  Task<IEnumerable<Models.ShippingOrder>> FindAsync(Specification<Models.ShippingOrder> specification, int pageIndex, int pageSize, CancellationToken cancellationToken);
  Task<IEnumerable<Models.ShippingOrder>> GetPagedShippingOrders(int pageIndex, int pageSize, CancellationToken cancellationToken);
  Task<long> GetTotalCountAsync(CancellationToken cancellationToken);
}
