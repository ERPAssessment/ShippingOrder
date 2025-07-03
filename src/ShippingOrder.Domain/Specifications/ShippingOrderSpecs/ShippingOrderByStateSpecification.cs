using ShippingOrder.Domain.Enums;
using ShippingOrder.Domain.Specifications.Shared;
using System.Linq.Expressions;

namespace ShippingOrder.Domain.Specifications.ShippingOrderSpecs;

public class ShippingOrderByStateSpecification : Specification<Models.ShippingOrder>
{
  private readonly ShippingOrderState _state;

  public ShippingOrderByStateSpecification(ShippingOrderState state)
  {
    _state = state;
  }

  public override Expression<Func<Models.ShippingOrder, bool>> ToExpression()
  {
    return po => po.DocumentState == _state;
  }
}
