using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using minimalAPIStructure.Endpoints.Orders.GetOrderById;
using MinimalAPIStructure.Data;
using MinimalAPIStructure.Models;

namespace minimalAPIStructure.Endpoints.Orders.CreateOrder
{
    using CreateOrderResult = Results<CreatedAtRoute<CreateOrderResponse>, ValidationProblem>;

    public static class CreateOrderHandler
    {
       public static async Task<CreateOrderResult> Handle(ApplicationDbContext db, CreateOrderRequest request)
        {
            // ensure customer exists
            // validate customer exists
            var customer = await db.Customers.FindAsync(request.CustomerId);
            if (customer is null)
            {
                return TypedResults.ValidationProblem(
                    new Dictionary<string, string[]> { [nameof(request.CustomerId)] = ["Customer not found."] }
                );
            }


            var product = await db.Products.FindAsync(request.ProductId);
            if (product is null)
            {
                return TypedResults.ValidationProblem(
                                    new Dictionary<string, string[]> { [nameof(request.ProductId)] = ["Product not found."] }
                                );
            }

            
                
            // map DTO → entity
            var order = new Order
            {
                CustomerId = request.CustomerId,
                ProductId = request.ProductId,
                DeliveryDate = request.DeliveryDate,
                Quantity = request.Quantity,
                UnitPrice = product.Price,
                Customer = customer,
                Product = product

                // assuming Product has a Customer navigation property
                // OrderDate will default to UtcNow via model
            };

            // save changes
            await db.Orders.AddAsync(order);
            await db.SaveChangesAsync();

            var createdOrder = new CreateOrderResponse
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

            return TypedResults.CreatedAtRoute(
                createdOrder,
                nameof(GetOrderByIdHandler),
                new RouteValueDictionary { ["id"] = createdOrder.Id }

                );
        }

       
        
    }
}
