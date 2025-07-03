using ShippingOrder.Domain.Exceptions;

namespace ShippingOrder.Domain.ValueObjects;
public record PurchaseGoodCode : ValueObject
{
  public string Code { get; }
  private PurchaseGoodCode(string code) => Code = code;
  public static PurchaseGoodCode Of(string value)
  {
    ArgumentNullException.ThrowIfNull(value);
    if (value == string.Empty)
    {
      throw new DomainException("PurchaseGoodCode cannot be empty.");
    }

    return new PurchaseGoodCode(value);
  }

  protected override IEnumerable<object> GetEqualityComponents()
  {
    yield return Code;
  }
}
