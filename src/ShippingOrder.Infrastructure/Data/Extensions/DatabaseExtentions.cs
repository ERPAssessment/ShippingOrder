using Microsoft.AspNetCore.Builder;

namespace ShippingOrder.Infrastructure.Data.Extensions;

public static class DatabaseExtentions
{
  public static async Task InitialiseDatabaseAsync(this WebApplication app)
  {
    using var scope = app.Services.CreateScope();

    var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

    context.Database.MigrateAsync().GetAwaiter().GetResult();

    await SeedAsync(context);
  }

  private static async Task SeedAsync(ApplicationDbContext context)
  {
    await SeedShippingOrdersWithItemsAsync(context);
  }

  private static async Task SeedShippingOrdersWithItemsAsync(ApplicationDbContext context)
  {
    if (!await context.ShippingOrders.AnyAsync())
    {
      await context.ShippingOrders.AddRangeAsync(InitialData.ShippingOrders);
      await context.SaveChangesAsync();
    }
  }
}
