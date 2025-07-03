using Models = ShippingOrder.Domain.Models;
namespace ShippingOrder.Application.Extenstions;

public static class ShippingOrdersExtenstion
{
  public static IEnumerable<ShippingOrderDto> ToShipppingOrdersDtoList(this IEnumerable<Models.ShippingOrder> ordersDB)
  {
    return ordersDB.Select(order => order.ToShippingOrderDto());
  }

  public static ShippingOrderDto ToShippingOrderDto(this Models.ShippingOrder order)
  {
    return new ShippingOrderDto(
        Id: order.Id.Value,
        PurchaseOrderNumber: order.PONumber.Value,
        ShippingOrderNumber: order.SHONumber.Value,
        DeliveryDate: order.DeliveryDate,
        PalletCounts: order.PalletsCount,
        DocumentState: order.DocumentState.ToString(),
        TotalPrice: order.TotalPrice.Amount,
        TotalItems: order.TotalPurchaseItemsCount,
        ShippingItems: order.ShippingItems.ToItemDtoList().ToList()
    );
  }
}
