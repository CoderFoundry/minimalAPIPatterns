# minimalAPIPatterns

A demonstration of two patterns for structuring ASP.NET Core Minimal APIs using extension methods.
These are examples of common project structures with the goal of demonstrating how to define
minimal API endpoints *without* cluttering `Program.cs`.

- **Controller-style pattern**: Defines endpoints in a way similar to MVC controllers by grouping
related routes and handlers.  
- **REPR pattern**: Follows the **Request**, **Endpoint**, **Response** structure for each 
resource, without external libraries.

This repository showcases three extension methods:

1. `MapCustomerEndpoints` – MVC-like grouping for Customer endpoints.  
2. `MapOrderEndpoints` – REPR pattern for Order endpoints.  
3. `MapProductEndpoints` – REPR pattern for Product endpoints.

## 📺 Video Walkthrough

Watch the full walkthrough on YouTube:

[Minimal API Patterns Walkthrough](VIDEO_URL)

*(Replace `VIDEO_URL` with the actual link to your YouTube video.)*

## 🚀 Getting Started

### Prerequisites

- [.NET 10 SDK](https://dotnet.microsoft.com/download/dotnet/10.0) or later  
- Visual Studio 2022 preview / Visual Studio Code  

### Running the Examples

1. Clone the repository:
   ```bash
   git clone https://github.com/CoderFoundry/minimalAPIPatterns.git
   cd minimalAPIPatterns
   ```

2. Run the project:
   ```bash
   cd minimalapistructure
   dotnet run 
   ```
  
3. View the API Documentation by visiting https://localhost:7127/ in your browser


## 🏗️ Patterns

### Customer Endpoints (Controller Pattern)
Defined in [CustomerEndpoints.cs](./minimalAPIStructure/Endpoints/CustomerEndpoints.cs) using the `MapCustomerEndpoints` extension method:

```csharp
public static class CustomerEndpoints
{
    public static IEndpointRouteBuilder MapCustomerEndpoints(this IEndpointRouteBuilder route)
    {
        var group = route.MapGroup("/customers").WithTags("Customers");

        group.MapGet("", GetCustomers);
        group.MapGet("{id:int}", GetCustomerById);
        group.MapPost("", CreateCustomer);
        group.MapPut("{id:int}", UpdateCustomer);
        group.MapDelete("{id:int}", DeleteCustomer);

        return group;
   }

    // Handler methods (GetCustomers, GetCustomerById, etc.) live in the same file.
    private static async Task<Ok<IEnumerable<CustomerResponse>>> GetCustomers(ICustomerService svc)
    {
        return TypedResults.Ok(
            await svc.GetCustomersAsync()
        );
    }
    // ...other methods below...
}
```

### Order Endpoints (REPR Pattern)
Defined in [OrderEndpoints.cs](./minimalAPIStructure/Endpoints/Orders/OrderEndpoints.cs) using the `MapOrderEndpoints` extension method:

 ```csharp
public static class OrderEndpoints
{
    public static IEndpointRouteBuilder MapOrderEndpoints(this IEndpointRouteBuilder route)
    {
        var group = route.MapGroup("/orders").WithTags("Orders");

        group.MapGet("", GetOrdersHandler.Handle);
        group.MapGet("{id:int}", GetOrderByIdHandler.Handle);
        group.MapPost("", CreateOrderHandler.Handle);
        group.MapPut("{id:int}", UpdateOrderHandler.Handle);
        group.MapDelete("{id:int}", DeleteOrderHandler.Handle);
       
        return group;
    }

    // Each endpoint has its own folder with its handler, request DTO, and response DTO
}
```
Each endpoint in the REPR pattern has its own folder for the endpoint,
located in [Endpoints/Orders](./minimalAPIStructure/Endpoints/Orders/).
