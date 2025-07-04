using ERP.Shared.CQRS;
using ShippingOrder.Application.Exceptions;

namespace ShippingOrder.Application.ShippingOrder.Commands.CloseShippingOrder
{
  internal class CloseShippingOrderHandler
  (IWriteShippingOrderRepository OrderRepository) :
   ICommandHandler<CloseShippingOrderCommand, CloseShippingOrderResult>
  {
    public async Task<CloseShippingOrderResult> Handle(CloseShippingOrderCommand request, CancellationToken cancellationToken)
    {
      var order = await CloseSHOOrder(Guid.Parse(request.OrderId), cancellationToken);

      await OrderRepository.Update(order, cancellationToken);

      return new CloseShippingOrderResult(true);
    }

    private async Task<Domain.Models.ShippingOrder> CloseSHOOrder(Guid Id, CancellationToken cancellationToken)
    {
      var orderId = ShippingOrderId.Of(Id);

      var order = await OrderRepository.GetById(orderId, cancellationToken);

      if (order is null)
      {
        throw new ShippingOrderNotFoundException(Id.ToString());
      }

      order.CloseShippingOrder();

      return order;
    }
  }
}
