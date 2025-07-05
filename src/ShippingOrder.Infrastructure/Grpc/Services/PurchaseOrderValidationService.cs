using PurchasingOrder.API.Protos;
using ShippingOrder.Application.Services;

namespace ShippingOrder.Infrastructure.GRPC.Services;

public class PurchaseOrderValidationService : IPurchaseOrderValidationService
{
  private readonly OrderProtoService.OrderProtoServiceClient _client;

  public PurchaseOrderValidationService(OrderProtoService.OrderProtoServiceClient client)
  {
    _client = client;
  }

  public async Task<bool> ValidateOrderAsync(string poNumber, List<(string Id, string Code, decimal Price)> items)
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

    var response = await _client.IsValidAndEligibleOrderAsync(request);
    return response.Success;
  }
}
