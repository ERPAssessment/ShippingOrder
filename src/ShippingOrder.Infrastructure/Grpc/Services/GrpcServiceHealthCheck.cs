using Grpc.Health.V1;
using Grpc.Net.Client;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace ShippingOrder.Infrastructure.Grpc.Services;

public class GrpcServiceHealthCheck : IHealthCheck
{
  private readonly GrpcChannel _channel;

  public GrpcServiceHealthCheck(IConfiguration configuration)
  {
    var address = configuration["PurchasingOrder:Uri"]!;
    _channel = GrpcChannel.ForAddress(address);
  }

  public async Task<HealthCheckResult> CheckHealthAsync(
      HealthCheckContext context,
      CancellationToken cancellationToken = default)
  {
    try
    {
      var client = new Health.HealthClient(_channel);
      var response = await client.CheckAsync(new HealthCheckRequest(), null, null, cancellationToken);

      return response.Status == HealthCheckResponse.Types.ServingStatus.Serving
          ? HealthCheckResult.Healthy()
          : HealthCheckResult.Unhealthy();
    }
    catch (Exception ex)
    {
      return HealthCheckResult.Unhealthy(exception: ex);
    }
  }
}
