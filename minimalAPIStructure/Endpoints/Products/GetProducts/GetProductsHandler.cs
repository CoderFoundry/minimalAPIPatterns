using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using MinimalAPIStructure.Data;

namespace minimalAPIStructure.Endpoints.Products.GetProducts
{
    public static class GetProductsHandler
    {
       public static async Task<Ok<IEnumerable<GetProductsResponse>>> Handle(ApplicationDbContext db)
        {
            var products = await db.Products.ToListAsync();
            var response = products.Select(p => new GetProductsResponse
            {
                Id = p.Id,
                Name = p.Name,
                Description = p.Description,
                Price = p.Price
            });
            return TypedResults.Ok(response);
        }

        
    }
}
