
using Microsoft.AspNetCore.Http.HttpResults;
using minimalAPIStructure.Endpoints.Products.GetProducts;
using minimalAPIStructure.Endpoints.Products.CreateProduct;
using minimalAPIStructure.Endpoints.Products.GetProductById;

using minimalAPIStructure.Models.DTO;
using minimalAPIStructure.Services;
using minimalAPIStructure.Endpoints.Products.DeleteProduct;
using minimalAPIStructure.Endpoints.Products.UpdateProduct;

namespace minimalAPIStructure.Endpoints.Products
{
    public static class ProductEndpoints
    {
        public static IEndpointRouteBuilder MapProductEndpoints(this IEndpointRouteBuilder route)
        {
            var group = route.MapGroup("/products")
                .WithTags("Products");

            group.MapGet("/", GetProductsHandler.Handle)
                .Produces<IEnumerable<GetProductsResponse>>(StatusCodes.Status200OK)
                .WithName(nameof(GetProductsHandler))
                .WithSummary("Get Products")
                .WithDescription("Get a list of all products.");

            group.MapGet("/{id:int}", GetProductByIdHandler.Handle)
                .Produces<GetProductByIdResponse>(StatusCodes.Status200OK)
                .Produces(StatusCodes.Status404NotFound)
                .WithName(nameof(GetProductByIdHandler))
                .WithSummary("Get Product by Id")
                .WithDescription("Get detailed information about a specific product.");

            group.MapPost("", CreateProductHandler.Handle)
                .Produces<CreateProductResponse>(StatusCodes.Status201Created)
                .WithName(nameof(CreateProductHandler))
                .WithSummary("Create Product")
                .WithDescription("Create a new product");

            group.MapPut("{id:int}", UpdateProductHandler.Handle)
                .Produces<UpdateProductResponse>(StatusCodes.Status200OK)
                .Produces(StatusCodes.Status404NotFound)
                .WithName(nameof(UpdateProductHandler))
                .WithSummary("Update Product by Id")
                .WithDescription("Update an existing product");

            group.MapDelete("{id:int}", DeleteProductHandler.Handle)
                .Produces(StatusCodes.Status204NoContent)
                .Produces(StatusCodes.Status404NotFound)
                .WithName(nameof(DeleteProductHandler))
                .WithSummary("Delete a product by Id.")    
                .WithDescription("Delete an existing Product");
               

            return group;
        }

       
    }
}
