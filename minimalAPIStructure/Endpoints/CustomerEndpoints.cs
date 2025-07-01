using Microsoft.AspNetCore.Http.HttpResults;
using minimalAPIStructure.Models.DTO;
using minimalAPIStructure.Services;
using System.Runtime.Intrinsics.Arm;

namespace minimalAPIStructure.Endpoints
{

    public static class CustomerEndpoints
    {
        public static IEndpointRouteBuilder MapCustomerEndpoints(this IEndpointRouteBuilder route)
        {
            var group = route.MapGroup("/customers")
            .WithTags("Customers");

            group.MapGet("", GetCustomers)
                .Produces<IEnumerable<CustomerResponse>>(StatusCodes.Status200OK)   
                .WithName(nameof(GetCustomers))
                .WithSummary("Get Customers")
                .WithDescription("Get a list of all customers.");
                       
            group.MapGet("{id:int}", GetCustomerById)
                .Produces<CustomerResponse>(StatusCodes.Status200OK)
                .Produces(StatusCodes.Status404NotFound)            
                .WithName(nameof(GetCustomerById))
                .WithSummary("Get Customer by Id")
                .WithDescription("Get detailed information about a specific customer.");

            group.MapPost("", CreateCustomer)
                .Produces<CustomerResponse>(StatusCodes.Status201Created)            
                .WithName(nameof(CreateCustomer))
                .WithSummary("Create Customer")
                .WithDescription("Create a new customer");

            group.MapPut("{id:int}", UpdateCustomer)
                .Produces<CustomerResponse>(StatusCodes.Status200OK)
                .Produces(StatusCodes.Status404NotFound)
                .WithName(nameof(UpdateCustomer))
                .WithDescription("Update an existing customer")               
                .WithSummary("Update Customer by Id");

            group.MapDelete("{id:int}", DeleteCustomer )
                .Produces(StatusCodes.Status204NoContent)
                .Produces(StatusCodes.Status404NotFound)            
                .WithName(nameof(DeleteCustomer)) 
                .WithDescription("Delete a customer by Id.")
                .WithSummary("Delete a customer");


            return group;

        }

        private static async Task<Ok<IEnumerable<CustomerResponse>>> GetCustomers(ICustomerService svc)
        {
            return TypedResults.Ok(await svc.GetCustomersAsync());
        }

        private static async Task<Results<Ok<CustomerResponse>, NotFound>> GetCustomerById(int id, ICustomerService svc)
        {
            var customer = await svc.GetCustomerByIdAsync(id);
            return customer is not null
                ? TypedResults.Ok(customer)
                : TypedResults.NotFound();
        }

        private static async Task<CreatedAtRoute<CustomerResponse>> CreateCustomer(CustomerRequest dto, ICustomerService svc)
        {
            var createdCustomer = await svc.CreateCustomerAsync(dto);
            return TypedResults.CreatedAtRoute(
                value: createdCustomer,
                routeName: "GetCustomerById",
                routeValues: new { id = createdCustomer.Id }
                );
        }

        private static async Task<Results<Ok<CustomerResponse>, NotFound>> UpdateCustomer(int id, CustomerRequest dto, ICustomerService svc)
        {
            var updated = await svc.UpdateCustomerAsync(id, dto);
            return updated is not null
                ? TypedResults.Ok(updated)
                : TypedResults.NotFound();
        }

        private static async Task<Results<NoContent, NotFound>> DeleteCustomer(int id, ICustomerService svc)
        {
            var deleted = await svc.DeleteCustomerAsync(id);
            return deleted
                ? TypedResults.NoContent()
                : TypedResults.NotFound();
        }
    }
}
