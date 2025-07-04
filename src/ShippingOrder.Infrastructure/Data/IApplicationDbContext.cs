namespace ShippingOrder.Infrastructure.Data;

public interface IApplicationDbContext
{
  public DbSet<Domain.Models.ShippingOrder> ShippingOrders { get; }
  public DbSet<ShippingItem> ShippingItems { get; }
  public DbSet<OutboxMessage> OutboxMessages { get; }
  Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}
