
using PurchasingOrder.Application.Extenstions;
using ShippingOrder.Application.Exceptions;
using ShippingOrder.Application.ShippingOrder.Queries.GetShippingOrderById;

namespace PurchasingOrder.Application.PurchaseOrders.Queries.GetPurchaseOrderById;

public class GetShippingOrderByIdHandler(IReadShippingOrderRepository orderRepository)
    : IQueryHandler<GetShippingOrderByIdQuery, GetShippingOrderByIdResults>
{
  public async Task<GetShippingOrderByIdResults> Handle(GetShippingOrderByIdQuery query, CancellationToken cancellationToken)
  {
    var orderId = ShippingOrderId.Of(Guid.Parse(query.Id));
    var order = await orderRepository.GetById(orderId, cancellationToken);

    if (order == null)
    {
      throw new ShippingOrderNotFoundException(query.Id);
    }

    return new GetShippingOrderByIdResults(order.ToShippingOrderDto());
  }
}