namespace ShippingOrder.Application.Data;

public interface IShippingNumberGenerator
{
  Task<ShippingOrderNumber> Generate();
}
