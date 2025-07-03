using ShippingOrder.Application.Data;
using ShippingOrder.Domain.Abstractions.Repositories;

namespace ShippingOrder.Infrastructure.Data.Repositories;
internal class WriteShippingOrderRepository
  (IApplicationDbContext dbContext)
  : IWriteShippingOrderRepository
{
  public async Task<Domain.Models.ShippingOrder> Add(Domain.Models.ShippingOrder shippingOrder, CancellationToken cancellationToken)
  {
    dbContext.ShippingOrders.Add(shippingOrder);
    await dbContext.SaveChangesAsync(cancellationToken);
    return shippingOrder;
  }

  public async Task<Domain.Models.ShippingOrder?> GetById(ShippingOrderId Id, CancellationToken cancellationToken)
  {
    var order = await dbContext.ShippingOrders
                         .FindAsync([Id], cancellationToken: cancellationToken);

    return order;
  }

  public async Task<Domain.Models.ShippingOrder> Update(Domain.Models.ShippingOrder shippingOrder, CancellationToken cancellationToken)
  {
    dbContext.ShippingOrders.Update(shippingOrder);
    await dbContext.SaveChangesAsync(cancellationToken);
    return shippingOrder;
  }
}
