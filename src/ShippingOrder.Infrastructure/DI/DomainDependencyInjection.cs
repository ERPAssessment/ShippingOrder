using ShippingOrder.Application.Data;
using ShippingOrder.Infrastructure.Data.Generators.ShippingNumberGenerator;

namespace ShippingOrder.Infrastructure.DI;

internal static class DomainDependencyInjection
{
  internal static IServiceCollection AddDomainServices(this IServiceCollection services)
  {
    services.AddSingleton<IShippingNumberGenerator, ShippingNumberGenerator>();
    return services;
  }
}
