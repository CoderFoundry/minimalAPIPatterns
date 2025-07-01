using Microsoft.AspNetCore.Http.HttpResults;
using MinimalAPIStructure.Data;

namespace minimalAPIStructure.Endpoints.Products.GetProductById
{
    public static class GetProductByIdHandler
    {

        public static async Task<Results<Ok<GetProductByIdResponse>, NotFound>> Handle(int id, ApplicationDbContext db)
        {
            var product = await db.Products.FindAsync(id);
            if (product == null)
            {
                return TypedResults.NotFound();
            }
            var response = new GetProductByIdResponse
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                Price = product.Price
            };
            return TypedResults.Ok(response);
        }
    }
}
