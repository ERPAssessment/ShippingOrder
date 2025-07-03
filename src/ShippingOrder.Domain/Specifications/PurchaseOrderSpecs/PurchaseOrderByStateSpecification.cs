using ShippingOrder.Domain.Enums;
using ShippingOrder.Domain.Specifications.Shared;
using System.Linq.Expressions;

namespace ShippingOrder.Domain.Specifications.PurchaseOrderSpecs;

public class PurchaseOrderByStateSpecification : Specification<Models.ShippingOrder>
{
  private readonly ShippingOrderState _state;

  public PurchaseOrderByStateSpecification(ShippingOrderState state)
  {
    _state = state;
  }

  public override Expression<Func<Models.ShippingOrder, bool>> ToExpression()
  {
    return po => po.DocumentState == _state;
  }
}
