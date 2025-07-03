using ShippingOrder.Application.Data;

namespace ShippingOrder.Infrastructure.Data.Generators.ShippingNumberGenerator;

internal class ShippingNumberGenerator : IShippingNumberGenerator
{
  public ShippingOrderNumber Generate()
  {
    return ShippingOrderNumber.Of($"SHO_{Guid.NewGuid()}");
  }
}


