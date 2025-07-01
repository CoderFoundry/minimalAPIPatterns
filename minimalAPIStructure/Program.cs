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

app.MapCustomerEndpoints();
app.MapProductEndpoints();
app.MapOrderEndpoints();


app.Run();


