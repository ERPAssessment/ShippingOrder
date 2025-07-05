using ShippingOrder.Infrastructure.DI;

namespace ShippingOrder.Infrastructure;

public static class DependencyInjection
{
  public static IServiceCollection AddInfrastructureServices(
      this IServiceCollection services,
      IConfiguration configuration)
  {
    services.AddDatabaseServices(configuration);
    services.AddRepositories();
    services.AddDomainServices();
    services.AddBackgroundJobs(configuration);
    services.AddGRPCServices();

    return services;
  }
}
