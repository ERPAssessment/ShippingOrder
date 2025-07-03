using ShippingOrder.Shared.Exceptions;

namespace ShippingOrder.Application.Exceptions;
public class ShippingOrderItemsCannotEmptyException : NotFoundException
{
  public ShippingOrderItemsCannotEmptyException() : base("Shipping Order cannot be have empty items")
  {
  }
}
