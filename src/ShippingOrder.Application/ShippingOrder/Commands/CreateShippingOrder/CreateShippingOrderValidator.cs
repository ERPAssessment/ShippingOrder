namespace ShippingOrder.Application.ShippingOrder.Commands.CreateShippingOrder;

internal class CreateShippingOrderValidator : AbstractValidator<CreateShippingOrderCommand>
{
  public CreateShippingOrderValidator()
  {
    RuleFor(x => x.Order.DeliveryDate)
            .GreaterThanOrEqualTo(DateTime.Today)
            .WithMessage("Delivery date cannot be in the past.");

    RuleFor(x => x.Order.PurchaseOrderNumber)
        .NotEmpty()
        .WithMessage("Purchase Order Number is required.");

    RuleFor(x => x.Order.PalletCounts)
        .GreaterThan(0)
        .WithMessage("Pallet count must be greater than 0.");

    RuleFor(x => x.Order.ShippingItems)
        .NotNull()
        .WithMessage("Shipping items list cannot be null.")
        .NotEmpty()
        .WithMessage("Shipping items list cannot be empty.");

  }
}
