using ShippingOrder.Domain.Abstractions.Repositories;
using ShippingOrder.Infrastructure.Data.Repositories;

namespace ShippingOrder.Infrastructure.DI;

internal static class RepositoryDependencyInjection
{
  internal static IServiceCollection AddRepositories(this IServiceCollection services)
  {
    services.AddScoped<IWriteShippingOrderRepository, WriteShippingOrderRepository>();
    services.AddScoped<IReadShippingOrderRepository, ReadShippingOrderRepository>();

    return services;
  }
}
