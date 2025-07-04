namespace ShippingOrder.Infrastructure.Data.Extensions;

internal class InitialData
{
  public static IEnumerable<Domain.Models.ShippingOrder> ShippingOrders =>
    new List<Domain.Models.ShippingOrder>
    {
        // Matches PO_003 (has 3 purchase items)
        new Func<Domain.Models.ShippingOrder>(() =>
        {
            var shippingOrder = Domain.Models.ShippingOrder.CreateShippingOrder(
                ShippingOrderId.Of(Guid.NewGuid()),
                ShippingOrderNumber.Of("SHO_001"),
                PurchaseOrderNumber.Of("PO_003"),
                DateTime.UtcNow.AddDays(2),
                3,
                PurchaseGoodCode.Of("PG004"),
                Money.Of(250));

            shippingOrder.AddShippingItem(PurchaseGoodCode.Of("PG001"), Money.Of(1500));
            shippingOrder.AddShippingItem(PurchaseGoodCode.Of("PG003"), Money.Of(800));

            return shippingOrder;
        })(),

        // Matches PO_004 (has 3 purchase items)
        new Func<Domain.Models.ShippingOrder>(() =>
        {
            var shippingOrder = Domain.Models.ShippingOrder.CreateShippingOrder(
                ShippingOrderId.Of(Guid.NewGuid()),
                ShippingOrderNumber.Of("SHO_002"),
                PurchaseOrderNumber.Of("PO_004"),
                DateTime.UtcNow.AddDays(3),
                1,
                PurchaseGoodCode.Of("PG001"),
                Money.Of(1350));

            shippingOrder.AddShippingItem(PurchaseGoodCode.Of("PG002"), Money.Of(65));
            shippingOrder.AddShippingItem(PurchaseGoodCode.Of("PG004"), Money.Of(300));

            return shippingOrder;
        })()
    };
};
