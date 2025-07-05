namespace ShippingOrder.Application.Services;

public interface IPurchaseOrderValidationService
{
  Task<bool> ValidateOrderAsync(string poNumber, List<(string Id, string Code, decimal Price)> items);

}
