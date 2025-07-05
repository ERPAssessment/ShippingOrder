using ERP.Shared.Exceptions;

namespace ShippingOrder.Application.Exceptions;

internal class ShippingOrderNotValidException : BadRequestException
{
  public ShippingOrderNotValidException(string id) : base("ShippingOrder", id)
  {
  }
}
