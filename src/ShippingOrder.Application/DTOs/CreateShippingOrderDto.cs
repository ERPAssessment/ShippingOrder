namespace ShippingOrder.Application.DTOs;

public record CreateShippingOrderDto(
  DateTime DeliveryDate,
  string PurchaseOrderNumber,
  int PalletCounts,
  List<ShippingItemDto> ShippingItems
);

