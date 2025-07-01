using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using MinimalAPIStructure.Data;

namespace minimalAPIStructure.Endpoints.Orders.UpdateOrder
{
    using UpdateOrderResult = Results<Ok<UpdateOrderResponse>, NotFound, BadRequest, ValidationProblem>;

    public static class UpdateOrderHandler
    {
      public static async Task<UpdateOrderResult> Handle(int id, UpdateOrderRequest request, ApplicationDbContext db)
        {
            var order = await db.Orders.FindAsync(id);

            if (order is null)
            {
                return TypedResults.NotFound();
            }


            var requestCustomer = await db.Customers.FindAsync(request.CustomerId);

            if (requestCustomer is null)
            {
                return TypedResults.BadRequest();
            }

            var requestProduct = await db.Products.FindAsync(request.ProductId); 

            if (requestProduct is null)
            {
                return TypedResults.BadRequest();
            }

            order.Product = requestProduct;
            order.Customer = requestCustomer;
            order.CustomerId = request.CustomerId;
            order.ProductId = request.ProductId;
            order.UnitPrice = requestProduct.Price;
            order.DeliveryDate = request.DeliveryDate;
            order.Quantity = request.Quantity;            

            await db.SaveChangesAsync();

            // re-project to DTO
           var response =  new UpdateOrderResponse
            {
                Id = order.Id,
                CustomerId = order.CustomerId,
                CustomerName = order.CustomerName,
                ProductId = order.ProductId,
                ProductName = order.ProductName,
                UnitPrice = order.UnitPrice,
                Quantity = order.Quantity,
                OrderDate = order.OrderDate,
                DeliveryDate = order.DeliveryDate,
                TotalPrice = order.TotalPrice
            };

            return TypedResults.Ok(response);
        }
    }

    
}