using ShippingOrder.Application.Exceptions;
using ShippingOrder.Application.Services;
using Models = ShippingOrder.Domain.Models;

namespace ShippingOrder.Application.ShippingOrder.Commands.CreateShippingOrder;

public class CreateShippingOrderHandler
  (
  IPurchaseOrderValidationService PurchaseOrderService,
  IWriteShippingOrderRepository OrderRepository,
  IShippingNumberGenerator ShippingOrderNumberGenerator
  ) :
   ICommandHandler<CreateShippingOrderCommand, CreateShippingOrderResult>
{
  public async Task<CreateShippingOrderResult> Handle(CreateShippingOrderCommand request, CancellationToken cancellationToken)
  {
    var shOrder = await CreateNewSHOrder(request.Order);
    await OrderRepository.Add(shOrder, cancellationToken);
    return new CreateShippingOrderResult(shOrder.Id.Value);
  }

  private async Task<Models.ShippingOrder> CreateNewSHOrder(CreateShippingOrderDto order)
  {
    if (order.ShippingItems.Count < 1)
    {
      throw new ShippingOrderItemsCannotEmptyException();
    }

    await ValidateOrderAsync(order);

    var FirstItem = order.ShippingItems[0];

    var SHO = Models.ShippingOrder.CreateShippingOrder(ShippingOrderId.Of(Guid.NewGuid()),
                                               await ShippingOrderNumberGenerator.Generate(),
                                               PurchaseOrderNumber.Of(order.PurchaseOrderNumber),
                                               order.DeliveryDate,
                                               order.PalletCounts,
                                               PurchaseGoodCode.Of(FirstItem.PurchaseGoodCode),
                                               Money.Of(FirstItem.Price)
                                             );


    order.ShippingItems.Remove(FirstItem);

    foreach (var ItemDto in order.ShippingItems)
    {
      SHO.AddShippingItem(PurchaseGoodCode.Of(ItemDto.PurchaseGoodCode),
                         Money.Of(ItemDto.Price));
    }

    return SHO;
  }

  async Task ValidateOrderAsync(CreateShippingOrderDto dto)
  {
    var items = dto.ShippingItems
        .Select(item => (item.Id.ToString(), item.PurchaseGoodCode, item.Price))
        .ToList();

    bool result = await PurchaseOrderService.ValidateOrderAsync(dto.PurchaseOrderNumber, items);
    if (!result)
    {
      throw new ShippingOrderNotValidException(dto.PurchaseOrderNumber);
    }
  }
}
