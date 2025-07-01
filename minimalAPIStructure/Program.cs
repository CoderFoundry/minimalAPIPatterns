using AuthDemoYT.Data;
using Microsoft.EntityFrameworkCore;
using minimalAPIStructure.Endpoints;
using minimalAPIStructure.Endpoints.Orders;
using minimalAPIStructure.Endpoints.Products;
using Microsoft.OpenApi.Models;
using minimalAPIStructure.Services;
using MinimalAPIStructure.Data;
using Scalar.AspNetCore;
using Microsoft.OpenApi.Interfaces;


var builder = WebApplication.CreateBuilder(args);


builder.Services.AddOpenApi( options =>
{
    options.AddDocumentTransformer((document, context, cancellationToken) =>
    {
        document.Info = new()
        {
            Title = "Minimal API Patterns | V1",
            Version = "v1",
            Description = """
                <img src="/images/CF_Logo_WO.png" height="120" />  
                
                Design Patterns for building minimal APIs
                """
        };        

        return Task.CompletedTask;
    });
});

var connectionString = DataUtility.GetConnectionString(builder.Configuration) ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

builder.Services.AddDbContextFactory<ApplicationDbContext>(options =>
    options.UseSqlite(connectionString));

builder.Services.AddScoped<ICustomerService, CustomerService>();

// enable validation for minimal APIs - for .net 10 and above 
builder.Services.AddValidation();

var app = builder.Build();

app.UseStaticFiles();

using (var scope = app.Services.CreateScope())
{
    await DataUtility.ManageDataAsync(scope.ServiceProvider);
}

app.MapOpenApi();
app.MapScalarApiReference("", opt =>
{
    opt.Title = "Minimal API Server";
    opt.Theme = ScalarTheme.Mars;
    
});

#region Customer Endpoints
//// GET /customers
//app.MapGet("/customers", async (CustomerService svc) =>
//    TypedResults.Ok(await svc.GetCustomersAsync())
//)
//.Produces<IEnumerable<CustomerResponseDTO>>(StatusCodes.Status200OK)
//.WithName("GetCustomers")
//.WithTags("Customers");

//// GET /customers/{id}
//app.MapGet("/customers/{id:int}", async Task<IResult> (int id, CustomerService svc) =>
//{
//    var customer = await svc.GetCustomerByIdAsync(id);
//    return customer is not null
//        ? TypedResults.Ok(customer)
//        : TypedResults.NotFound();
//})
//.Produces<CustomerResponseDTO>(StatusCodes.Status200OK)
//.Produces(StatusCodes.Status404NotFound)
//.WithName("GetCustomerById")
//.WithTags("Customers");

//// POST /customers
//app.MapPost("/customers", async (CustomerRequestDTO dto, CustomerService svc) =>
//{
//    var created = await svc.AddCustomerAsync(dto);
//    return TypedResults.Created($"/customers/{created.Id}", created);
//})
//.Produces<CustomerResponseDTO>(StatusCodes.Status201Created)
//.WithName("AddCustomer")
//.WithTags("Customers");

//// PUT /customers/{id}
//app.MapPut("/customers/{id:int}", async Task<IResult> (int id, CustomerRequestDTO dto, CustomerService svc) =>
//{
//    var updated = await svc.UpdateCustomerAsync(id, dto);
//    return updated is not null
//        ? TypedResults.Ok(updated)
//        : TypedResults.NotFound();
//})
//.Produces<CustomerResponseDTO>(StatusCodes.Status200OK)
//.Produces(StatusCodes.Status404NotFound)
//.WithName("UpdateCustomer")
//.WithTags("Customers");

//// DELETE /customers/{id}
//app.MapDelete("/customers/{id:int}", async Task<IResult> (int id, CustomerService svc) =>
//{
//    var deleted = await svc.DeleteCustomerAsync(id);
//    return deleted
//        ? TypedResults.NoContent()
//        : TypedResults.NotFound();
//})
//.Produces(StatusCodes.Status204NoContent)
//.Produces(StatusCodes.Status404NotFound)
//.WithName("DeleteCustomer")
//.WithTags("Customers");

#endregion customer endpoints

app.MapCustomerEndpoints();
app.MapProductEndpoints();
app.MapOrderEndpoints();


app.Run();


