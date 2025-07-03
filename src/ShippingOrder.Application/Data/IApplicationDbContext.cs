using Microsoft.EntityFrameworkCore;
using ShippingOrder.Domain.Models;

namespace ShippingOrder.Application.Data;

public interface IApplicationDbContext
{
  public DbSet<Domain.Models.ShippingOrder> ShippingOrders { get; }
  public DbSet<ShippingItem> ShippingItems { get; }
  Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}
