using ShippingOrder.Application.ShippingOrder.Queries.GetShippingOrderById;

namespace PurchasingOrder.API.EndPoints;

public record GetShippingOrderByIdResponse(ShippingOrderDto Order);

public class GetShippingOrderById : ICarterModule
{
  public void AddRoutes(IEndpointRouteBuilder app)
  {
    app.MapGet("/GetShippingOrderById/{Id}", async (string Id, ISender sender) =>
    {
      var result = await sender.Send(new GetShippingOrderByIdQuery(Id));

      var response = result.Adapt<GetShippingOrderByIdResponse>();

      return Results.Ok(response);
    })
           .WithName("GetPurchaseOrdersById")
           .Produces<GetShippingOrderByIdResponse>(StatusCodes.Status200OK)
           .ProducesProblem(StatusCodes.Status400BadRequest)
           .ProducesProblem(StatusCodes.Status404NotFound)
           .WithSummary("GetPurchaseOrdersById")
           .WithDescription("GetPurchaseOrdersById");
  }
}
