using ShippingOrder.Domain.Exceptions;
using ShippingOrder.Domain.Specifications.Shared;
using System.Linq.Expressions;

namespace ShippingOrder.Domain.Specifications.ShippingOrderSpecs;

public class ShippingOrderByDateRangeSpecification : Specification<Models.ShippingOrder>
{
  private readonly DateTime _startDate;
  private readonly DateTime _endDate;

  public ShippingOrderByDateRangeSpecification(DateTime startDate, DateTime endDate)
  {
    if (startDate > endDate)
      throw new DomainException("Start date cannot be later than end date.");

    _startDate = startDate;
    _endDate = endDate;
  }

  public override Expression<Func<Models.ShippingOrder, bool>> ToExpression()
  {
    return po => po.DeliveryDate >= _startDate && po.DeliveryDate <= _endDate;
  }
}
