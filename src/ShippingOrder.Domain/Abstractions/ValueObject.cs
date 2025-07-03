namespace ShippingOrder.Domain.Abstractions;

public abstract record ValueObject : IEquatable<ValueObject>
{
  protected static bool EqualOperator(ValueObject left, ValueObject right)
  {
    if (ReferenceEquals(left, null) ^ ReferenceEquals(right, null))
      return false;

    return ReferenceEquals(left, null) || left.Equals(right!);
  }

  protected static bool NotEqualOperator(ValueObject left, ValueObject right)
      => !EqualOperator(left, right);

  protected abstract IEnumerable<object> GetEqualityComponents();

  public virtual bool Equals(ValueObject? other)
  {
    if (other is null || other.GetType() != GetType())
      return false;

    return GetEqualityComponents().SequenceEqual(other.GetEqualityComponents());
  }

  public override int GetHashCode()
  {
    return GetEqualityComponents()
        .Where(x => x != null)
        .Aggregate(17, (current, obj) => current * 31 + obj.GetHashCode());
  }

  public ValueObject GetCopy() => (ValueObject)MemberwiseClone();

}