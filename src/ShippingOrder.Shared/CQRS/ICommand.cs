using MediatR;

namespace ShippingOrder.Shared.CQRS;

public interface ICommand : ICommand<Unit>
{
}

public interface ICommand<out TResponse> : IRequest<TResponse>
{
}
