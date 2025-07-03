namespace ShippingOrder.Infrastructure.Data.Extensions;

internal class InitialData
{
  public static IEnumerable<Domain.Models.ShippingOrder> ShippingOrders =>
    new List<Domain.Models.ShippingOrder>
    {
        // Matches PO_001 (has 3 purchase items)
        new Func<Domain.Models.ShippingOrder>(() =>
        {
            var shippingOrder = Domain.Models.ShippingOrder.CreateShippingOrder(
                ShippingOrderId.Of(Guid.NewGuid()),
                ShippingOrderNumber.Of("SHO_001"),
                PurchaseOrderNumber.Of("PO_001"),
                DateTime.UtcNow.AddDays(2),
                3,
                PurchaseGoodCode.Of("PG003"),
                Money.Of(953));

            shippingOrder.AddShippingItem(PurchaseGoodCode.Of("PG001"), Money.Of(1200));
            shippingOrder.AddShippingItem(PurchaseGoodCode.Of("PG002"), Money.Of(50));

            return shippingOrder;
        })(),

        // Matches PO_002 (has 3 purchase items)
        new Func<Domain.Models.ShippingOrder>(() =>
        {
            var shippingOrder = Domain.Models.ShippingOrder.CreateShippingOrder(
                ShippingOrderId.Of(Guid.NewGuid()),
                ShippingOrderNumber.Of("SHO_002"),
                PurchaseOrderNumber.Of("PO_002"),
                DateTime.UtcNow.AddDays(3),
                1,
                PurchaseGoodCode.Of("PG002"),
                Money.Of(123));

            shippingOrder.AddShippingItem(PurchaseGoodCode.Of("PG003"), Money.Of(75));
            shippingOrder.AddShippingItem(PurchaseGoodCode.Of("PG004"), Money.Of(200));

            return shippingOrder;
        })()
    };
};
