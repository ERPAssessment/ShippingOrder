namespace ShippingOrder.Application.DTOs;

public record ShippingOrderDto(
  Guid Id,
  DateTime DeliveryDate,
  string ShippingOrderNumber,
  string PurchaseOrderNumber,
  int PalletCounts,
  string DocumentState,
  decimal TotalPrice,
  int TotalItems,
  List<ShippingItemDto> ShippingItems
);

