﻿using System.Reflection;

namespace ShippingOrder.Infrastructure.Data;

public class ApplicationDbContext : DbContext, IApplicationDbContext
{
  public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
  : base(options) { }


  public DbSet<Domain.Models.ShippingOrder> ShippingOrders => Set<Domain.Models.ShippingOrder>();
  public DbSet<ShippingItem> ShippingItems => Set<ShippingItem>();

  public DbSet<OutboxMessage> OutboxMessages => Set<OutboxMessage>();

  protected override void OnModelCreating(ModelBuilder builder)
  {
    builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    base.OnModelCreating(builder);
  }
}
