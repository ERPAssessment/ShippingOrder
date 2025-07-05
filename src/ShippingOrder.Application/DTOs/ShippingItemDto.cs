namespace ShippingOrder.Application.DTOs;
public record ShippingItemDto(
    string Id,
    string PurchaseGoodCode,
    decimal Price
);