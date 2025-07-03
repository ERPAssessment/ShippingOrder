using ShippingOrder.Domain.ValueObjects;
namespace ShippingOrder.Application.Data;

public interface IShippingNumberGenerator
{
  ShippingOrderNumber Generate();
}
