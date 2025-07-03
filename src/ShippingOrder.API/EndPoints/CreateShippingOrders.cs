using ShippingOrder.Application.ShippingOrder.Commands.CreateShippingOrder;

namespace PurchasingOrder.API.EndPoints;

public record CreateShippingOrderResponse(Guid OrderId);
public record CreateShippingOrderRequest(CreateShippingOrderDto Orders);

public class CreateShippingOrders : ICarterModule
{
  public void AddRoutes(IEndpointRouteBuilder app)
  {
    app.MapPost("/CreateShippingOrder", async (CreateShippingOrderRequest request, ISender sender) =>
    {
      var command = request.Adapt<CreateShippingOrderCommand>();
      var result = await sender.Send(command);
      var response = result.Adapt<CreateShippingOrderResponse>();
      return Results.Created($"/GetShippingOrderById/{response.OrderId}", response);
    })
           .WithName("CreateShippingOrder")
           .Produces<CreateShippingOrderResponse>(StatusCodes.Status201Created)
           .ProducesProblem(StatusCodes.Status400BadRequest)
           .ProducesProblem(StatusCodes.Status404NotFound)
           .WithSummary("CreateShippingOrder")
           .WithDescription("CreateShippingOrder");
  }
}