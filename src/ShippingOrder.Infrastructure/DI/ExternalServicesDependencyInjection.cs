using PurchasingOrder.API.Protos;
using RabbitMQ.Client;
using ShippingOrder.Application.Services;
using ShippingOrder.Infrastructure.Grpc.Services;
using ShippingOrder.Infrastructure.GRPC.Services;

namespace ShippingOrder.Infrastructure.DI;

internal static class ExternalServicesDependencyInjection
{
  internal static IServiceCollection AddGRPCServices(this IServiceCollection services, IConfiguration configuration)
  {
    services.AddGrpcClient<OrderProtoService.OrderProtoServiceClient>(options =>
    {
      options.Address = new Uri(configuration["PurchasingOrder:Uri"]!);
    }).ConfigurePrimaryHttpMessageHandler(() =>
    {
      var handler = new HttpClientHandler
      {
        ServerCertificateCustomValidationCallback =
          HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
      };

      return handler;
    });
    ;

    services.AddScoped<IPurchaseOrderValidationService, PurchaseOrderValidationService>();

    return services;
  }

  internal static void AddDIHealthChecks(this IServiceCollection services, IConfiguration configuration)
  {
    // Get RabbitMQ config values
    var rabbitHost = configuration["MessageBroker:Host"];
    var rabbitUser = configuration["MessageBroker:UserName"];
    var rabbitPassword = configuration["MessageBroker:Password"];

    // Combine into connection string
    var rabbitMqConnectionString = $"{rabbitHost}?username={rabbitUser}&password={rabbitPassword}";

    services.AddSingleton(sp =>
    {
      var factory = new ConnectionFactory
      {

        Uri = new Uri(rabbitMqConnectionString),
      };
      return factory.CreateConnectionAsync().GetAwaiter().GetResult();
    })

    .AddHealthChecks()
    .AddSqlServer(configuration.GetConnectionString("Database")!)
    .AddCheck<GrpcServiceHealthCheck>("grpcHealth");


  }

}
