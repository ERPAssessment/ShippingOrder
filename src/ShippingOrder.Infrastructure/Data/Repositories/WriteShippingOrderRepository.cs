using ShippingOrder.Application.Data;
using ShippingOrder.Domain.Abstractions.Repositories;

namespace ShippingOrder.Infrastructure.Data.Repositories;
internal class WriteShippingOrderRepository
  (IApplicationDbContext dbContext)
  : IWriteShippingOrderRepository
{
  public async Task<IEnumerable<Domain.Models.ShippingOrder>> Add(IEnumerable<Domain.Models.ShippingOrder> shippingOrders, CancellationToken cancellationToken)
  {
    dbContext.ShippingOrders.AddRange(shippingOrders);
    await dbContext.SaveChangesAsync(cancellationToken);
    return shippingOrders;
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
