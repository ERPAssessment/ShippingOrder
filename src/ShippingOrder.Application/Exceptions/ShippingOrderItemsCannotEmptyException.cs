using ShippingOrder.Shared.Exceptions;

namespace PurchasingOrder.Application.Exceptions;
public class ShippingOrderItemsCannotEmptyException : NotFoundException
{
  public ShippingOrderItemsCannotEmptyException() : base("Shipping Order cannot be have empty items")
  {
  }
}
