using PurchasingOrder.API.Protos;
using ShippingOrder.Application.Services;

namespace ShippingOrder.Infrastructure.GRPC.Services;

public class PurchaseOrderValidationService
  (ILogger<PurchaseOrderValidationService> logger,
  OrderProtoService.OrderProtoServiceClient client)
  : IPurchaseOrderValidationService
{
  public async Task<bool> ValidateOrderAsync(string poNumber, List<(string Id, string Code, decimal Price)> items)
  {
    try
    {
      var request = new msgIsValidAndEligibleOrderRequest
      {
        PurchaseOrderNumber = poNumber
      };

      foreach (var item in items)
      {
        request.Items.Add(new msgPurchaseItem
        {
          Id = item.Id,
          GoodCode = item.Code,
          Price = (double)item.Price
        });
      }

      var response = await client.IsValidAndEligibleOrderAsync(request);
      return response.Success;
    }
    catch (Exception ex)
    {
      logger.LogError(ex.Message);
      return false;
    }
  }
}
