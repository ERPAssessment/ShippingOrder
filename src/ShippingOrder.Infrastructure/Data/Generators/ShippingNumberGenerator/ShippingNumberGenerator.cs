using ShippingOrder.Application.Data;

namespace ShippingOrder.Infrastructure.Data.Generators.ShippingNumberGenerator;

internal class ShippingNumberGenerator : IShippingNumberGenerator
{
  public Task<ShippingOrderNumber> Generate()
  {
    return Task.FromResult(ShippingOrderNumber.Of($"SHO_{Guid.NewGuid()}"));
  }
}


