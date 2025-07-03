namespace ShippingOrder.Application.DTOs;
public record ShippingItemDto(
    Guid Id,
    string PurchaseGoodCode,
    decimal Price
);