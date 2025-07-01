using minimalAPIStructure.Models.DTO;

namespace minimalAPIStructure.Services
{
    public interface ICustomerService
    {
        Task<CustomerResponse> CreateCustomerAsync(CustomerRequest dto);
        Task<bool> DeleteCustomerAsync(int id);
        Task<CustomerResponse?> GetCustomerByIdAsync(int id);
        Task<IEnumerable<CustomerResponse>> GetCustomersAsync();
        Task<CustomerResponse?> UpdateCustomerAsync(int id, CustomerRequest dto);
    }
}