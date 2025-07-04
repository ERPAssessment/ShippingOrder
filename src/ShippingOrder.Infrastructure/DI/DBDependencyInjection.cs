using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore.Diagnostics;
using ShippingOrder.Infrastructure.Data.Interceptors;

namespace ShippingOrder.Infrastructure.DI;

internal static class DBDependencyInjection
{
  private const string DATABASE_CONNECTION_STRING_KEY = "Database";

  internal static IServiceCollection AddDatabaseServices(
      this IServiceCollection services,
      IConfiguration configuration)
  {
    var connectionString = configuration.GetConnectionString(DATABASE_CONNECTION_STRING_KEY)
        ?? throw new InvalidOperationException($"Connection string '{DATABASE_CONNECTION_STRING_KEY}' not found.");

    services.AddScoped<ISaveChangesInterceptor, AuditableEntityInterceptor>();
    services.AddScoped<ISaveChangesInterceptor, DispatchDomainEventsInterceptor>();

    services.AddDbContext<ApplicationDbContext>((serviceProvider, options) =>
    {
      options.AddInterceptors(serviceProvider.GetServices<ISaveChangesInterceptor>());
      options.UseSqlServer(connectionString, sqlOptions =>
      {
        sqlOptions.EnableRetryOnFailure(
            maxRetryCount: 3,
            maxRetryDelay: TimeSpan.FromSeconds(5),
            errorNumbersToAdd: null);
        sqlOptions.CommandTimeout(30);
      });

      var environment = serviceProvider.GetService<IWebHostEnvironment>();
    });

    services.AddScoped<IApplicationDbContext, ApplicationDbContext>();

    return services;
  }
}
