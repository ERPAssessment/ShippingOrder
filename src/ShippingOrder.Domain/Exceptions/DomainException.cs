namespace ShippingOrder.Domain.Exceptions;

public class DomainException : Exception
{
  public DomainException(string message)
      : base($"Domain Exception: \"{message}\" throws from Domain Layer.")
  {
  }

  public DomainException(string message, Exception innerException)
      : base($"Domain Exception: \"{message}\" throws from Domain Layer.", innerException)
  { }

}
