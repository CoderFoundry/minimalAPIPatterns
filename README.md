# minimalAPIPatterns

A demonstration of two patterns for structuring ASP.NET Core Minimal APIs using extension methods.

- **Controller-style pattern**: Defines endpoints in a way similar to MVC controllers by grouping related routes and handlers.  
- **REPR pattern**: Follows the **Request**, **Endpoint**, **Response** structure for each resource, without external libraries.

This repository showcases three extension methods:

1. `MapCustomerEndpoints` – MVC-like grouping for Customer endpoints.  
2. `MapOrderEndpoints` – REPR pattern for Order endpoints.  
3. `MapProductEndpoints` – REPR pattern for Product endpoints.

---

## 📺 Video Walkthrough

Watch the full walkthrough on YouTube:

[Minimal API Patterns Walkthrough](VIDEO_URL)

*(Replace `VIDEO_URL` with the actual link to your YouTube video.)*

---

## 🚀 Getting Started

### Prerequisites

- [.NET 10 SDK](https://dotnet.microsoft.com/download/dotnet/10.0) or later  
- Visual Studio 2022 preview / Visual Studio Code  

### Running the Examples

1. **Clone the repository**:
   ```bash
   git clone https://github.com/CoderFoundry/minimalAPIPatterns.git
   cd minimalAPIPatterns

2. **Run the project**:
   ```bash
   cd minimalapistructure
   dotnet run 
  
3. **Run the API**:
  Open you are browser at 
  https://localhost:7127/

## 🏗️ Patterns


1. **Customer Endpoints (Controller-Style)**
  Defined in Endpoints/CustomerEndpoints.cs using the MapCustomerEndpoints extension method:

  ```csharp
public static class CustomerEndpoints
{
    public static IEndpointRouteBuilder MapCustomerEndpoints(this IEndpointRouteBuilder route)
    {
        var group = route.MapGroup("/customers")
        .WithTags("Customers");

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

    // Handler methods (GetAllCustomers, GetCustomerById, etc.) live in the same file.
    private static async Task<Ok<IEnumerable<CustomerResponse>>> GetCustomers(ICustomerService svc)
    {
      return TypedResults.Ok(await svc.GetCustomersAsync());
    }
    // other methods below
}
```

2. **Order Endpoints (REPR Pattern)**
Defined in Endpoints/Orders/OrderEndpoints.cs using the MapOrderEndpoints extension method:

 ```csharp
  public static class OrderEndpoints
{
    public static IEndpointRouteBuilder MapOrderEndpoints(this IEndpointRouteBuilder route)
    {
        var group = route.MapGroup("/orders")
            .WithTags("Orders");

        group.MapGet("", GetOrdersHandler.Handle)
            .Produces<IEnumerable<GetOrdersResponse>>(StatusCodes.Status200OK)
            .WithName(nameof(GetOrdersHandler))
            .WithSummary("Get Orders")
            .WithDescription("Get a list of all orders."); ;

        group.MapGet("{id:int}", GetOrderByIdHandler.Handle)
            .Produces<GetOrderByIdResponse>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound)
            .WithName(nameof(GetOrderByIdHandler))
            .WithSummary("Get Order by Id")
            .WithDescription("Get detailed information about a specific order."); ;

        group.MapPost("", CreateOrderHandler.Handle)
             .Produces<CreateOrderResponse>(StatusCodes.Status201Created)
             .Produces(StatusCodes.Status400BadRequest)
             .WithName(nameof(CreateOrderHandler))
             .WithSummary("Create Order")
             .WithDescription("Create a new order");

        group.MapPut("{id:int}", UpdateOrderHandler.Handle)
            .Produces<UpdateOrderResponse>(StatusCodes.Status201Created)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status404NotFound)
            .WithName(nameof(UpdateOrderHandler))
            .WithSummary("Update Order")
            .WithDescription("Update an existing order");

        group.MapDelete("{id:int}", DeleteOrderHandler.Handle)
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status404NotFound)
            .WithName(nameof(DeleteOrderHandler))
            .WithSummary("Delete Order")
            .WithDescription("Delete an existing order by Id.");
       
        return group;
    }
}

// Handler methods (GetProducts, GetProductById, CreateProduct, etc.)
//endpoint handler lives in its own folder.
```
Each REPR extension method cleanly separates the Request DTOs, the endpoint mappings, and the Response DTOs for clarity and consistency.
