using ShippingOrder.Domain.Enums;
using ShippingOrder.Domain.Events;
using ShippingOrder.Domain.Exceptions;
using ShippingOrder.Domain.ValueObjects;

namespace ShippingOrder.Domain.Models;

public class ShippingOrder : Aggregate<ShippingOrderId>
{
  private readonly List<ShippingItem> _shippingItems = [];
  public ShippingOrderNumber SHONumber { get; private set; } = default!;
  public PurchaseOrderNumber PONumber { get; private set; } = default!;
  public DateTime DeliveryDate { get; private set; } = default!;
  public int PalletsCount { get; private set; } = default!;
  public ShippingOrderState DocumentState { get; private set; } = ShippingOrderState.Draft;
  public IReadOnlyList<ShippingItem> ShippingItems => _shippingItems.AsReadOnly();
  public Money TotalPrice => Money.Of(ShippingItems.Sum(x => x.Price.Amount));
  public int TotalPurchaseItemsCount => ShippingItems.Count;

  public static ShippingOrder CreateShippingOrder(
      ShippingOrderId id,
      ShippingOrderNumber shoNumber,
      PurchaseOrderNumber poNumber,
      DateTime deliveryDate,
      int palletsCount,
      PurchaseGoodCode goodCode,
      Money price)
  {
    var SHOOrder = new ShippingOrder
    {
      Id = id,
      SHONumber = shoNumber,
      PONumber = poNumber,
      DeliveryDate = deliveryDate,
      PalletsCount = palletsCount,
      DocumentState = ShippingOrderState.Created,
    };

    SHOOrder.AddShippingItem(goodCode, price);
    SHOOrder.AddDomainEvent(new ShippingOrderCreatedDomainEvent(SHOOrder));

    return SHOOrder;
  }

  public void CloseShippingOrder()
  {
    CheckCanClose();

    DocumentState = ShippingOrderState.Closed;
    AddDomainEvent(new ShippingOrderClosedDomainEvent(this));
  }

  private void CheckCanClose()
  {
    if (DocumentState != ShippingOrderState.Created)
      throw new DomainException("Only Shipped orders can be closed.");
  }

  public void AddShippingItem(PurchaseGoodCode goodCode, Money price)
  {
    CanAddShippingItem();

    var item = new ShippingItem(Id, goodCode, price);
    _shippingItems.Add(item);
  }

  private void CanAddShippingItem()
  {
    if (DocumentState != ShippingOrderState.Created)
      throw new DomainException("Items can only be added to a new Shipping Order.");
  }
}
