namespace ShippingOrder.Domain.ValueObjects;
public record Money : ValueObject
{
  public decimal Amount { get; private set; }
  private Money(decimal value) => Amount = value;

  public static Money Of(decimal value)
  {
    if (value < 0)
      throw new ArgumentException("Money cannot be negative");

    return new Money(value);
  }

  protected override IEnumerable<object> GetEqualityComponents()
  {
    yield return Amount;
  }

  public static bool operator >=(Money left, Money right)
  {
    if (left is null)
      return right is null;

    if (right is null)
      return true;

    return left.Amount >= right.Amount;
  }

  public static bool operator <=(Money left, Money right)
  {
    if (left is null)
      return true;

    if (right is null)
      return false;

    return left.Amount <= right.Amount;
  }

}
