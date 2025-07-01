using Microsoft.AspNetCore.Http.HttpResults;
using minimalAPIStructure.Endpoints.Products.GetProductById;
using minimalAPIStructure.Models;
using MinimalAPIStructure.Data;

namespace minimalAPIStructure.Endpoints.Products.CreateProduct
{
    using CreateProductResult = CreatedAtRoute<CreateProductResponse>;

    public static class CreateProductHandler
    {
        public static async Task<CreateProductResult> Handle(CreateProductRequest request, ApplicationDbContext db)
        {
            var product = new Product
            {
                Name = request.Name,
                Description = request.Description,
                Price = request.Price
            };

            db.Products.Add(product);
            await db.SaveChangesAsync();

            var created = new CreateProductResponse
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                Price = product.Price
            };


            return TypedResults.CreatedAtRoute(
                created,
                nameof(GetProductByIdHandler),
                new RouteValueDictionary { ["id"] = created.Id });

        }
    }
}
