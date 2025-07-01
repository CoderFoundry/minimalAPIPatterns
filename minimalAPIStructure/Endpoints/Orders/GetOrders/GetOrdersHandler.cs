using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using MinimalAPIStructure.Data;


namespace minimalAPIStructure.Endpoints.Orders.GetOrders
{
    public static class GetOrdersHandler
    {
    
        public static async Task<Ok<IEnumerable<GetOrdersResponse>>> Handle(ApplicationDbContext db, string? customerName = null)
        {

            // include Customer so CustomerName (First + Last) is available
            var query = db.Orders
                                .Include(o => o.Customer)
                                .Include(o => o.Product)
                                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(customerName))
            {
                customerName = customerName.Trim();
                query = query.Where(o =>
                    (o.Customer.FirstName + " " + o.Customer.LastName)
                    .Contains(customerName));
            }

            var orders = await query.ToListAsync();

           var response =  orders.Select(o => new GetOrdersResponse
            {
                Id = o.Id,
                CustomerId = o.CustomerId,
                CustomerName = o.CustomerName,
                ProductId = o.ProductId,
                ProductName = o.Product.Name,
                UnitPrice = o.UnitPrice,
                Quantity = o.Quantity,
                OrderDate = o.OrderDate,
                DeliveryDate = o.DeliveryDate,
                TotalPrice = o.TotalPrice
            });

            return TypedResults.Ok(response);
        }

        

    }
}
