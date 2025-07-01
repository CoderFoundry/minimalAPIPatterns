using Microsoft.EntityFrameworkCore;
using minimalAPIStructure.Models.DTO;
using MinimalAPIStructure.Data;
using MinimalAPIStructure.Models;
using System.Text.Json;

namespace minimalAPIStructure.Services;

public class CustomerService(ApplicationDbContext context) : ICustomerService
{


    /// <summary>
    /// Retrieves all customers.
    /// </summary>
    public async Task<IEnumerable<CustomerResponse>> GetCustomersAsync()
    {
        var list = await context.Customers
                                 .AsNoTracking()
                                 .ToListAsync();

        return list.Select(c => new CustomerResponse
        {
            Id = c.Id,
            FirstName = c.FirstName,
            LastName = c.LastName,
            Email = c.Email,
            Address = c.Address,
            Address2 = c.Address2,
            City = c.City,
            State = c.State,
            ZipCode = c.ZipCode
        });
    }

    /// <summary>
    /// Retrieves a customer by Id.
    /// </summary>
    public async Task<CustomerResponse?> GetCustomerByIdAsync(int id)
    {
        var c = await context.Customers
                               .AsNoTracking()
                               .FirstOrDefaultAsync(x => x.Id == id);
        if (c is null) return null;

        return new CustomerResponse
        {
            Id = c.Id,
            FirstName = c.FirstName,
            LastName = c.LastName,
            Email = c.Email,
            Address = c.Address,
            Address2 = c.Address2,
            City = c.City,
            State = c.State,
            ZipCode = c.ZipCode
        };
    }

    /// <summary>
    /// Creates a new customer.
    /// </summary>
    public async Task<CustomerResponse> CreateCustomerAsync(CustomerRequest dto)
    {
        var entity = new Customer
        {
            FirstName = dto.FirstName,
            LastName = dto.LastName,
            Email = dto.Email,
            Address = dto.Address,
            Address2 = dto.Address2,
            City = dto.City,
            State = dto.State,
            ZipCode = dto.ZipCode
        };

        context.Customers.Add(entity);
        await context.SaveChangesAsync();

        return new CustomerResponse
        {
            Id = entity.Id,
            FirstName = entity.FirstName,
            LastName = entity.LastName,
            Email = entity.Email,
            Address = entity.Address,
            Address2 = entity.Address2,
            City = entity.City,
            State = entity.State,
            ZipCode = entity.ZipCode
        };
    }

    /// <summary>
    /// Updates an existing customer.
    /// </summary>
    public async Task<CustomerResponse?> UpdateCustomerAsync(int id, CustomerRequest dto)
    {
        var entity = await context.Customers.FindAsync(id);
        if (entity is null) return null;

        entity.FirstName = dto.FirstName;
        entity.LastName = dto.LastName;
        entity.Email = dto.Email;
        entity.Address = dto.Address;
        entity.Address2 = dto.Address2;
        entity.City = dto.City;
        entity.State = dto.State;
        entity.ZipCode = dto.ZipCode;

        await context.SaveChangesAsync();

        return new CustomerResponse
        {
            Id = entity.Id,
            FirstName = entity.FirstName,
            LastName = entity.LastName,
            Email = entity.Email,
            Address = entity.Address,
            Address2 = entity.Address2,
            City = entity.City,
            State = entity.State,
            ZipCode = entity.ZipCode
        };
    }

    /// <summary>
    /// Deletes a customer by Id.
    /// </summary>
    public async Task<bool> DeleteCustomerAsync(int id)
    {
        var entity = await context.Customers.FindAsync(id);
        if (entity is null) return false;

        context.Customers.Remove(entity);
        await context.SaveChangesAsync();
        return true;
    }


}
