using ShippingOrder.Domain.ValueObjects;

namespace ShippingOrder.Domain.Abstractions.Repositories;

public interface IWriteShippingOrderRepository
{
  Task<Models.ShippingOrder> Add(Models.ShippingOrder shippingOrder, CancellationToken cancellationToken);
  Task<Models.ShippingOrder> Update(Models.ShippingOrder shippingOrder, CancellationToken cancellationToken);
  Task<Models.ShippingOrder?> GetById(ShippingOrderId Id, CancellationToken cancellationToken);
}
