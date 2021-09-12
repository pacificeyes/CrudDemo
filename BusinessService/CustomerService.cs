using System.Collections.Generic;
using System.Threading.Tasks;
using BusinessEntities;
using Crud.Data.Interface;

namespace BusinessService
{
    public class CustomerService : ICustomerService
    {
        private readonly ICustomerRepository _repository;
        
        public CustomerService(ICustomerRepository repository)
        {
            _repository = repository;
        }

        /// <summary>
        /// Gets All the existing Customers List by making a call to
        /// repository layer
        /// </summary>
        /// <returns></returns>
        public async Task<List<CustomerDto>> GetAllCustomersAsync()
        {
            return await _repository.GetAllCustomersAsync();
        }

        /// <summary>
        /// Gets the Customer information based on the given id
        /// by making a call to repository layer
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<CustomerDto> GetCustomerAsync(int id)
        {
            return await _repository.GetCustomerAsync(id);
        }

        /// <summary>
        /// Creates a new customer as per the customer dto data and returns
        /// its id by making a call to repository layer
        /// </summary>
        /// <param name="customer"></param>
        /// <returns></returns>
        public async Task<int> CreateCustomerAsync(CreateCustomerDto customer)
        {
            return await _repository.CreateCustomerAsync(customer);
        }

        /// <summary>
        /// Updates the Customer data based on the customer dto
        /// inforation by making a call to the repository layer
        /// </summary>
        /// <param name="customer"></param>
        /// <returns></returns>
        public async Task UpdateCustomerAsync(CustomerDto customer)
        {
            await _repository.UpdateCustomerAsync(customer);
        }

        /// <summary>
        /// Deletes the customer as per the given id
        /// by making a call to the repository layer
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task DeleteCustomerAsync(int id)
        {
            await _repository.DeleteCustomerAsync(id);
        }
    }
}
