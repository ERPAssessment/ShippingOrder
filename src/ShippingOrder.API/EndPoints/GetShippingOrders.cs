using ShippingOrder.Application.ShippingOrder.Queries.GetShippingOrders;

namespace ShippingOrder.API.EndPoints;

public record GetShippingOrdersRequest(int PageIndex = 0,
    int PageSize = 10,
    DateTime? StartDate = null,
    DateTime? EndDate = null,
    string? State = null);


public record GetShippingOrdersResponse(PaginatedResult<ShippingOrderDto> Orders);


public class GetShippingOrders : ICarterModule
{
  public void AddRoutes(IEndpointRouteBuilder app)
  {
    app.MapGet("/GetShippingOrders", async ([AsParameters] GetShippingOrdersRequest request, ISender sender) =>
    {
      var query = new GetShippingOrdersQuery(
          new PaginationRequest(request.PageIndex, request.PageSize),
          request.StartDate,
          request.EndDate,
          request.State
      );

      var result = await sender.Send(query);

      var response = result.Adapt<GetShippingOrdersResponse>();

      return Results.Ok(response);
    })
           .WithName("GetShippingOrders")
           .Produces<GetShippingOrdersResponse>(StatusCodes.Status200OK)
           .ProducesProblem(StatusCodes.Status400BadRequest)
           .ProducesProblem(StatusCodes.Status404NotFound)
           .WithSummary("GetShippingOrders")
           .WithDescription("GetShippingOrders");
  }
}
