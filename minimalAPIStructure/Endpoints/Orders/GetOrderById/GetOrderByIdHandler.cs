using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using minimalAPIStructure.Endpoints.Orders.UpdateOrder;
using MinimalAPIStructure.Data;


namespace minimalAPIStructure.Endpoints.Orders.GetOrderById
{
    using GetOrderByIdResult = Results<Ok<GetOrderByIdResponse>, NotFound>;

    public static class GetOrderByIdHandler
    {
        public static async Task<GetOrderByIdResult> Handle(ApplicationDbContext db, int id)
        {

            // Include Customer so CustomerName will be available
            var order = await db.Orders
                              .Include(x => x.Customer)
                              .FirstOrDefaultAsync(x => x.Id == id);

            if (order is null) return TypedResults.NotFound();

            var response = new GetOrderByIdResponse
            {
                Id = order.Id,
                CustomerId = order.CustomerId,
                CustomerName = order.CustomerName,
                UnitPrice = order.UnitPrice,
                Quantity = order.Quantity,
                OrderDate = order.OrderDate,
                DeliveryDate = order.DeliveryDate,
                TotalPrice = order.TotalPrice
            };


            return response is not null
                ? TypedResults.Ok(response)
                : TypedResults.NotFound();
        }


    }
}
