using ShippingOrder.Domain.Exceptions;

namespace ShippingOrder.Domain.ValueObjects;

public record ShippingItemId : ValueObject
{
  public Guid Value { get; }
  private ShippingItemId(Guid value) => Value = value;
  public static ShippingItemId Of(Guid value)
  {
    ArgumentNullException.ThrowIfNull(value);
    if (value == Guid.Empty)
    {
      throw new DomainException("ShippingItemId cannot be empty.");
    }

    return new ShippingItemId(value);
  }

  protected override IEnumerable<object> GetEqualityComponents()
  {
    yield return Value;
  }
}
