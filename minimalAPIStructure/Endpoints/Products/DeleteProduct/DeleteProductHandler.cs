using Microsoft.AspNetCore.Http.HttpResults;
using MinimalAPIStructure.Data;

namespace minimalAPIStructure.Endpoints.Products.DeleteProduct
{
    using DeleteProductResults = Results<NoContent, NotFound>;
    public static class DeleteProductHandler
    {
        public static async Task<DeleteProductResults> Handle(ApplicationDbContext db , int id )
        {
            var product = await db.Products.FindAsync(id);
            if (product is null)
            {
                return TypedResults.NotFound();
            }

            db.Products.Remove(product);
            await db.SaveChangesAsync();

            return TypedResults.NoContent();
        }
    }
}
