namespace ShippingOrder.Domain.UnitTests.Models;

public class ShippingOrderTests
{
  private ShippingOrder.Domain.Models.ShippingOrder CreateTestShippingOrder()
  {
    var id = ShippingOrderId.Of(Guid.NewGuid());
    var shoNumber = ShippingOrderNumber.Of("SHO123");
    var poNumber = PurchaseOrderNumber.Of("PO123");
    var deliveryDate = DateTime.UtcNow.AddDays(1);
    var palletsCount = 5;
    var goodCode = PurchaseGoodCode.Of("GC001");
    var price = Money.Of(100m);
    return ShippingOrder.Domain.Models.ShippingOrder.CreateShippingOrder(id, shoNumber, poNumber, deliveryDate, palletsCount, goodCode, price);
  }

  [Fact]
  public void CreateShippingOrder_WithValidParameters_ShouldSucceed()
  {
    // Arrange
    var id = ShippingOrderId.Of(Guid.NewGuid());
    var shoNumber = ShippingOrderNumber.Of("SHO123");
    var poNumber = PurchaseOrderNumber.Of("PO123");
    var deliveryDate = DateTime.UtcNow.AddDays(1);
    var palletsCount = 5;
    var goodCode = PurchaseGoodCode.Of("GC001");
    var price = Money.Of(100m);

    // Act
    var shippingOrder = ShippingOrder.Domain.Models.ShippingOrder.CreateShippingOrder(id, shoNumber, poNumber, deliveryDate, palletsCount, goodCode, price);

    // Assert
    Assert.Equal(id, shippingOrder.Id);
    Assert.Equal(shoNumber, shippingOrder.SHONumber);
    Assert.Equal(poNumber, shippingOrder.PONumber);
    Assert.Equal(deliveryDate, shippingOrder.DeliveryDate);
    Assert.Equal(palletsCount, shippingOrder.PalletsCount);
    Assert.Equal(ShippingOrderState.Created, shippingOrder.DocumentState);
    Assert.Single(shippingOrder.ShippingItems);
    var item = shippingOrder.ShippingItems[0];
    Assert.Equal(goodCode, item.PurchaseGoodCode);
    Assert.Equal(price, item.Price);
    Assert.Single(shippingOrder.DomainEvents);
    Assert.IsType<ShippingOrderCreatedDomainEvent>(shippingOrder.DomainEvents[0]);
  }

  [Fact]
  public void CloseShippingOrder_WhenStateIsCreated_ShouldSetStateToClosed()
  {
    // Arrange
    var shippingOrder = CreateTestShippingOrder();

    // Act
    shippingOrder.CloseShippingOrder();

    // Assert
    Assert.Equal(ShippingOrderState.Closed, shippingOrder.DocumentState);
    Assert.Equal(2, shippingOrder.DomainEvents.Count);
    Assert.IsType<ShippingOrderClosedDomainEvent>(shippingOrder.DomainEvents[1]);
  }

  [Fact]
  public void CloseShippingOrder_WhenStateIsClosed_ShouldThrowDomainException()
  {
    // Arrange
    var shippingOrder = CreateTestShippingOrder();
    shippingOrder.CloseShippingOrder(); // First close

    // Act & Assert
    Assert.Throws<DomainException>(() => shippingOrder.CloseShippingOrder());
  }

  [Fact]
  public void AddShippingItem_WhenStateIsCreated_ShouldAddItem()
  {
    // Arrange
    var shippingOrder = CreateTestShippingOrder();
    var goodCode = PurchaseGoodCode.Of("GC002");
    var price = Money.Of(200m);

    // Act
    shippingOrder.AddShippingItem(goodCode, price);

    // Assert
    Assert.Equal(2, shippingOrder.ShippingItems.Count);
    var item = shippingOrder.ShippingItems[1];
    Assert.Equal(goodCode, item.PurchaseGoodCode);
    Assert.Equal(price, item.Price);
  }

  [Fact]
  public void AddShippingItem_WhenStateIsClosed_ShouldThrowDomainException()
  {
    // Arrange
    var shippingOrder = CreateTestShippingOrder();
    shippingOrder.CloseShippingOrder();
    var goodCode = PurchaseGoodCode.Of("GC002");
    var price = Money.Of(200m);

    // Act & Assert
    Assert.Throws<DomainException>(() => shippingOrder.AddShippingItem(goodCode, price));
  }

  [Fact]
  public void TotalPrice_ShouldReturnSumOfItemPrices()
  {
    // Arrange
    var shippingOrder = CreateTestShippingOrder();
    shippingOrder.AddShippingItem(PurchaseGoodCode.Of("GC002"), Money.Of(200m));

    // Act
    var totalPrice = shippingOrder.TotalPrice;

    // Assert
    Assert.Equal(Money.Of(300m), totalPrice);
  }

  [Fact]
  public void TotalPurchaseItemsCount_ShouldReturnItemCount()
  {
    // Arrange
    var shippingOrder = CreateTestShippingOrder();
    shippingOrder.AddShippingItem(PurchaseGoodCode.Of("GC002"), Money.Of(200m));

    // Act
    var itemCount = shippingOrder.TotalPurchaseItemsCount;

    // Assert
    Assert.Equal(2, itemCount);
  }
}