using ShippingOrder.Domain.ValueObjects;

namespace ShippingOrder.Domain.Models;

public class ShippingItem : Entity<ShippingItemId>
{
  internal ShippingItem(ShippingOrderId shippingOrderId, PurchaseGoodCode purchaseGoodCode, Money price)
  {
    Id = ShippingItemId.Of(Guid.NewGuid());
    PurchaseGoodCode = purchaseGoodCode;
    ShippingOrderId = shippingOrderId;
    Price = price;
  }

  public ShippingOrderId ShippingOrderId { get; private set; } = default!;
  public PurchaseGoodCode PurchaseGoodCode { get; private set; } = default!;
  public Money Price { get; private set; } = default!;
}
