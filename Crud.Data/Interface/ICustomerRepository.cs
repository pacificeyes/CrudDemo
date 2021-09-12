using System.Collections.Generic;
using System.Threading.Tasks;
using BusinessEntities;

namespace Crud.Data.Interface
{
    public interface ICustomerRepository
    {
        Task<List<CustomerDto>> GetAllCustomersAsync();
        Task<CustomerDto> GetCustomerAsync(int id);
        Task<int> CreateCustomerAsync(CreateCustomerDto customer);
        Task DeleteCustomerAsync(int id);
        Task UpdateCustomerAsync(CustomerDto customer);
    }
}
