using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ShippingOrder.Application.Data;
using ShippingOrder.Domain.Abstractions.Repositories;
using ShippingOrder.Infrastructure.Data;
using ShippingOrder.Infrastructure.Data.Generators.ShippingNumberGenerator;
using ShippingOrder.Infrastructure.Data.Interceptors;
using ShippingOrder.Infrastructure.Data.Repositories;

namespace ShippingOrder.Infrastructure;

public static class DependencyInjection
{
  public static IServiceCollection AddInfrastructureServices
         (this IServiceCollection services, IConfiguration configuration)
  {
    var connectionString = configuration.GetConnectionString("Database");

    // Add services to the container.
    services.AddScoped<ISaveChangesInterceptor, AuditableEntityInterceptor>();
    services.AddScoped<ISaveChangesInterceptor, DispatchDomainEventsInterceptor>();

    services.AddDbContext<ApplicationDbContext>((sp, options) =>
    {
      options.AddInterceptors(sp.GetServices<ISaveChangesInterceptor>());
      options.UseSqlServer(connectionString);
    });

    services.AddScoped<IApplicationDbContext, ApplicationDbContext>();
    services.AddScoped<IWriteShippingOrderRepository, WriteShippingOrderRepository>();
    services.AddScoped<IReadShippingOrderRepository, ReadShippingOrderRepository>();

    services.AddSingleton<IShippingNumberGenerator, ShippingNumberGenerator>();

    return services;
  }
}