namespace ShippingOrder.Application.Extenstions;

public static class SHOItemExtenstion
{
  public static IEnumerable<ShippingItemDto> ToItemDtoList(this IEnumerable<ShippingItem> items)
  {
    return items.Select(item => new ShippingItemDto(
        Id: item.Id.Value,
        PurchaseGoodCode: item.PurchaseGoodCode.Code,
        Price: item.Price.Amount
    ));
  }

}
