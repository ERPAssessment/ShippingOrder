using ShippingOrder.Domain.Exceptions;

namespace ShippingOrder.Domain.ValueObjects;

public record ShippingOrderNumber : ValueObject
{
  public string Value { get; }
  private ShippingOrderNumber(string value) => Value = value;
  public static ShippingOrderNumber Of(string value)
  {
    ArgumentNullException.ThrowIfNull(value);
    if (value == string.Empty)
    {
      throw new DomainException("ShippingOrderNumber cannot be empty.");
    }

    return new ShippingOrderNumber(value);
  }

  protected override IEnumerable<object> GetEqualityComponents()
  {
    yield return Value;
  }
}
