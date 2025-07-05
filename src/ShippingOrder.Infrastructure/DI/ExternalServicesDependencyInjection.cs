using PurchasingOrder.API.Protos;
using ShippingOrder.Application.Services;
using ShippingOrder.Infrastructure.GRPC.Services;

namespace ShippingOrder.Infrastructure.DI
{
  internal static class ExternalServicesDependencyInjection
  {
    internal static IServiceCollection AddGRPCServices(this IServiceCollection services)
    {
      services.AddGrpcClient<OrderProtoService.OrderProtoServiceClient>(options =>
      {
        options.Address = new Uri("https://localhost:5050");
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
  }
}
