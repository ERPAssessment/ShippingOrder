using FluentValidation;

namespace ShippingOrder.Application.ShippingOrder.Commands.CreateShippingOrder;

public class CreateShippingOrderValidator : AbstractValidator<CreateShippingOrderCommand>
{
  public CreateShippingOrderValidator()
  {
    RuleFor(x => x.SHOrder.DeliveryDate)
            .GreaterThanOrEqualTo(DateTime.Today)
            .WithMessage("Delivery date cannot be in the past.");

    RuleFor(x => x.SHOrder.PurchaseOrderNumber)
        .NotEmpty()
        .WithMessage("Purchase Order Number is required.");

    RuleFor(x => x.SHOrder.PalletCounts)
        .GreaterThan(0)
        .WithMessage("Pallet count must be greater than 0.");

    RuleFor(x => x.SHOrder.ShippingItems)
        .NotNull()
        .WithMessage("Shipping items list cannot be null.")
        .NotEmpty()
        .WithMessage("Shipping items list cannot be empty.");

  }
}
