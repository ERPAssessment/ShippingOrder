using ShippingOrder.Application.ShippingOrder.Commands.CloseShippingOrder;

namespace ShippingOrder.API.EndPoints;

public record CloseShippingOrderResponse(bool Result);
public record CloseShippingOrderRequest(string OrderId);

public class CloseShippingOrder : ICarterModule
{
  public void AddRoutes(IEndpointRouteBuilder app)
  {
    app.MapPost("/CloseShippingOrder", async (CloseShippingOrderRequest request, ISender sender) =>
    {
      var command = request.Adapt<CloseShippingOrderCommand>();

      var result = await sender.Send(command);

      var response = result.Adapt<CloseShippingOrderResponse>();

      return Results.Created($"/GetShippingOrderById/{request.OrderId}", response);
    })
           .WithName("CloseShippingOrder")
           .Produces<CloseShippingOrderResponse>(StatusCodes.Status200OK)
           .ProducesProblem(StatusCodes.Status400BadRequest)
           .ProducesProblem(StatusCodes.Status404NotFound)
           .WithSummary("CloseShippingOrder")
           .WithDescription("CloseShippingOrder");
  }
}
