using ShippingOrder.Shared.Exceptions;

namespace ShippingOrder.Application.Exceptions;

internal class ShippingOrderNotFoundException : NotFoundException
{
  public ShippingOrderNotFoundException(string id) : base("ShippingOrder", id)
  {
  }
}
