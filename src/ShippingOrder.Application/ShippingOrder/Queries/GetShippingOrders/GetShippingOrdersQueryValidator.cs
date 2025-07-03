using FluentValidation;
using ShippingOrder.Domain.Enums;

namespace PurchasingOrder.Application.PurchaseOrders.Queries.GetPurchaseOrders;

public class GetShippingOrdersQueryValidator : AbstractValidator<GetShippingOrdersQuery>
{
  public GetShippingOrdersQueryValidator()
  {
    RuleFor(x => x.PaginationRequest)
         .NotNull()
         .WithMessage("Pagination must be provided.");

    RuleFor(x => x.PaginationRequest.PageIndex)
        .GreaterThanOrEqualTo(0)
        .WithMessage("PageIndex must be 0 or greater.");

    RuleFor(x => x.PaginationRequest.PageSize)
        .GreaterThan(0)
        .WithMessage("Page Size must be greater than 0.");

    When(x => x.StartDate.HasValue && x.EndDate.HasValue, () =>
    {
      RuleFor(x => x.EndDate)
          .GreaterThanOrEqualTo(x => x.StartDate)
          .WithMessage("EndDate must be greater than or equal to StartDate.");
    });

    RuleFor(x => x.State)
        .Must(state => string.IsNullOrEmpty(state) || Enum.TryParse<ShippingOrderState>(state, true, out _))
        .WithMessage($"State must be a valid ShippingOrderState enum value. Available values are: {GetEnumNames()}");
  }

  private string GetEnumNames()
  {
    return string.Join(", ", Enum.GetNames(typeof(ShippingOrderState)));
  }
}
