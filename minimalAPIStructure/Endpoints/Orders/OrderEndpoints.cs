using Microsoft.AspNetCore.Http.HttpResults;
using minimalAPIStructure.Endpoints.Orders.DeleteOrder;
using minimalAPIStructure.Endpoints.Orders.GetOrderById;
using minimalAPIStructure.Endpoints.Orders.GetOrders;
using minimalAPIStructure.Endpoints.Orders.CreateOrder;
using minimalAPIStructure.Endpoints.Orders.UpdateOrder;


namespace minimalAPIStructure.Endpoints.Orders
{
    public static class OrderEndpoints
    {
        public static IEndpointRouteBuilder MapOrderEndpoints(this IEndpointRouteBuilder route)
        {
            var group = route.MapGroup("/orders")
                .WithTags("Orders");

            group.MapGet("", GetOrdersHandler.Handle)
                .Produces<IEnumerable<GetOrdersResponse>>(StatusCodes.Status200OK)
                .WithName(nameof(GetOrdersHandler))
                .WithSummary("Get Orders")
                .WithDescription("Get a list of all orders."); ;

            group.MapGet("{id:int}", GetOrderByIdHandler.Handle)
                .Produces<GetOrderByIdResponse>(StatusCodes.Status200OK)
                .Produces(StatusCodes.Status404NotFound)
                .WithName(nameof(GetOrderByIdHandler))
                .WithSummary("Get Order by Id")
                .WithDescription("Get detailed information about a specific order."); ;

            group.MapPost("", CreateOrderHandler.Handle)
                 .Produces<CreateOrderResponse>(StatusCodes.Status201Created)
                 .Produces(StatusCodes.Status400BadRequest)
                 .WithName(nameof(CreateOrderHandler))
                 .WithSummary("Create Order")
                 .WithDescription("Create a new order");

            group.MapPut("{id:int}", UpdateOrderHandler.Handle)
                .Produces<UpdateOrderResponse>(StatusCodes.Status201Created)
                .Produces(StatusCodes.Status400BadRequest)
                .Produces(StatusCodes.Status404NotFound)
                .WithName(nameof(UpdateOrderHandler))
                .WithSummary("Update Order")
                .WithDescription("Update an existing order");

            group.MapDelete("{id:int}", DeleteOrderHandler.Handle)
                .Produces(StatusCodes.Status204NoContent)
                .Produces(StatusCodes.Status404NotFound)
                .WithName(nameof(DeleteOrderHandler))
                .WithSummary("Delete Order")
                .WithDescription("Delete an existing order by Id.");
           
            return group;
        }
    }
}
