namespace ShippingOrder.Application.ShippingOrder.Commands.CloseShippingOrder;

internal class CloseShippingOrderValidator : AbstractValidator<CloseShippingOrderCommand>
{
  public CloseShippingOrderValidator()
  {
    RuleFor(x => x.OrderId)
             .NotEmpty()
             .WithMessage("Shipping Order Id should not be empty")
             .Must(BeValidGuid)
             .WithMessage("Shipping Order Id must be a valid GUID format");
  }

  private bool BeValidGuid(string id)
  {
    if (string.IsNullOrEmpty(id))
      return false;

    return Guid.TryParse(id, out _);
  }
}
