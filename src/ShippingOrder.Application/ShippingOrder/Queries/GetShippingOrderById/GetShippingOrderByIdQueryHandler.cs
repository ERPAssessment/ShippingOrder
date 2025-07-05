using ShippingOrder.Application.Exceptions;
using ShippingOrder.Application.Extenstions;

namespace ShippingOrder.Application.ShippingOrder.Queries.GetShippingOrderById;

internal class GetShippingOrderByIdQueryHandler(IReadShippingOrderRepository orderRepository)
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