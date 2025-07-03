using ShippingOrder.Domain.Exceptions;

namespace ShippingOrder.Domain.ValueObjects;

public record ShippingOrderId : ValueObject
{
  public Guid Value { get; }
  private ShippingOrderId(Guid value) => Value = value;
  public static ShippingOrderId Of(Guid value)
  {
    ArgumentNullException.ThrowIfNull(value);
    if (value == Guid.Empty)
    {
      throw new DomainException("ShippingOrderId cannot be empty.");
    }

    return new ShippingOrderId(value);
  }

  protected override IEnumerable<object> GetEqualityComponents()
  {
    yield return Value;
  }
}
