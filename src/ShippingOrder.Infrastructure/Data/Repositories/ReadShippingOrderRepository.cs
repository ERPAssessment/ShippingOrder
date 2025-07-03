using ShippingOrder.Application.Data;
using ShippingOrder.Domain.Abstractions.Repositories;
using ShippingOrder.Domain.Specifications.Shared;

namespace ShippingOrder.Infrastructure.Data.Repositories;

internal class ReadShippingOrderRepository(IApplicationDbContext dbContext)
  : IReadShippingOrderRepository
{
  public async Task<IEnumerable<Domain.Models.ShippingOrder>> FindAsync(Specification<Domain.Models.ShippingOrder> specification, int pageIndex, int pageSize, CancellationToken cancellationToken)
  {
    return await dbContext.ShippingOrders
               .AsNoTracking()
               .Include(o => o.ShippingItems)
               .Where(specification.ToExpression())
               .OrderByDescending(o => o.DeliveryDate)
               .Skip(pageSize * pageIndex)
               .Take(pageSize)
               .ToListAsync(cancellationToken);
  }

  public async Task<Domain.Models.ShippingOrder?> GetById(ShippingOrderId Id, CancellationToken cancellationToken)
  {
    var order = await dbContext.ShippingOrders
                  .AsNoTracking()
                  .Include(o => o.ShippingItems)
                  .FirstOrDefaultAsync(sho => sho.Id == Id, cancellationToken);

    return order;
  }

  public async Task<IEnumerable<Domain.Models.ShippingOrder>> GetPagedShippingOrders(int pageIndex, int pageSize, CancellationToken cancellationToken)
  {
    return await dbContext.ShippingOrders
              .AsNoTracking()
              .Include(o => o.ShippingItems)
              .OrderByDescending(o => o.DeliveryDate)
              .Skip(pageSize * pageIndex)
              .Take(pageSize)
              .ToListAsync(cancellationToken);
  }

  public async Task<long> GetTotalCountAsync(CancellationToken cancellationToken)
  {
    return await dbContext.ShippingOrders
                  .AsNoTracking()
                  .LongCountAsync(cancellationToken);
  }
}
