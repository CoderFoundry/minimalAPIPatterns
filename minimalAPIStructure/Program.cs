using Microsoft.EntityFrameworkCore;
using minimalAPIStructure.Data;
using minimalAPIStructure.Endpoints;
using minimalAPIStructure.Endpoints.Orders;
using minimalAPIStructure.Endpoints.Products;
using minimalAPIStructure.Extensions;
using minimalAPIStructure.Services;
using MinimalAPIStructure.Data;
using Scalar.AspNetCore;


var builder = WebApplication.CreateBuilder(args);

builder.Services.ConfigureOpenApi();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite("Data Source=MinApiDemo.db"));

builder.Services.AddScoped<ICustomerService, CustomerService>();

// enable validation for minimal APIs - for .net 10 and above 
builder.Services.AddValidation();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    await DataUtility.ManageDataAsync(scope.ServiceProvider);
}

app.UseStaticFiles();
app.MapScalar();

app.MapCustomerEndpoints();
app.MapProductEndpoints();
app.MapOrderEndpoints();

app.Run();


