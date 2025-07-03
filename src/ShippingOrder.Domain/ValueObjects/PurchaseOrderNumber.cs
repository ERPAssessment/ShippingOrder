namespace ShippingOrder.Domain.ValueObjects;
public record PurchaseOrderNumber : ValueObject
{
  public string Value { get; }
  private PurchaseOrderNumber(string value) => Value = value;
  public static PurchaseOrderNumber Of(string value)
  {
    ArgumentNullException.ThrowIfNull(value);
    return new PurchaseOrderNumber(value);
  }

  protected override IEnumerable<object> GetEqualityComponents()
  {
    yield return Value;
  }
}
