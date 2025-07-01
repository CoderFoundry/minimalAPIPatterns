using Microsoft.AspNetCore.Http.HttpResults;
using minimalAPIStructure.Endpoints.Orders.UpdateOrder;
using MinimalAPIStructure.Data;

namespace minimalAPIStructure.Endpoints.Products.UpdateProduct
{
    using UpdateProductResult = Results<Ok<UpdateProductResponse>, NotFound, BadRequest, ValidationProblem>;
    
    public static class UpdateProductHandler
    {
        public static async Task<UpdateProductResult> Handle(int id, UpdateProductRequest request, ApplicationDbContext db)
        {
            var product = await db.Products.FindAsync(id);
            if (product is null)
            {
                return TypedResults.NotFound();
            }

            product.Name = request.Name;
            product.Description = request.Description;
            product.Price = request.Price;
            
            db.Products.Update(product);
            await db.SaveChangesAsync();
            
            var updated = new UpdateProductResponse
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                Price = product.Price
            };

            return TypedResults.Ok(updated);
        }
    }
}
