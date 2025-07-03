using FluentValidation;
using ShippingOrder.Application.ShippingOrder.Queries.GetShippingOrderById;

namespace PurchasingOrder.Application.PurchaseOrders.Queries.GetPurchaseOrderById;

public class GetShippingOrderByIdValidator : AbstractValidator<GetShippingOrderByIdQuery>
{
  public GetShippingOrderByIdValidator()
  {
    RuleFor(x => x.Id)
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