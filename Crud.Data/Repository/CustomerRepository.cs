using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BusinessEntities;
using Crud.Data.Interface;
using Crud.Data.Models;

namespace Crud.Data.Repository
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly CustomerContext _customerContext;
        private const string _CustomerErrorMessage = "The customer with Id - {0} does not exist";
        
        public CustomerRepository(CustomerContext customerContext)
        {
            _customerContext = customerContext;
        }

        /// <summary>
        /// Gets All the existing Customers List from database
        /// </summary>
        /// <returns></returns>
        public async Task<List<CustomerDto>> GetAllCustomersAsync()
        {
            var query = _customerContext.Customers
                .Where(x => x.IsActive == true)
                .OrderBy(x => x.Id)
                .Select(x =>
                new CustomerDto
                {
                    Id = x.Id,
                    FirstName = x.FirstName,
                    LastName = x.LastName
                });

            //The Task.FromResult has been used instead of .ToListAsync()
            //just to ensure that the unit tests can be written
            //however I prefer using .ToListAsync()
            var customers = await Task.FromResult(query.ToList());

            if (!customers.Any())
                throw new CustomException("There are no customers available to display");

            return customers;
        }

        /// <summary>
        /// Gets the Customer information based on the given id
        /// from database
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<CustomerDto> GetCustomerAsync(int id)
        {
            var query = _customerContext.Customers
                .Where(x => x.Id == id && x.IsActive.Value)
                .Select(x => new CustomerDto
                {
                    Id = x.Id,
                    FirstName = x.FirstName,
                    LastName = x.LastName
                });

            //The Task.FromResult has been used instead of .FirstOrDefaultAsync()
            //just to ensure that the unit tests can be written
            //however I prefer using .FirstOrDefaultAsync()
            var customer = await Task.FromResult(query.FirstOrDefault());

            if (customer == null)
                throw new CustomException(string.Format(_CustomerErrorMessage, id));

            return customer;
        }

        /// <summary>
        /// Creates a new customer as per the customer dto data and returns
        /// its id from the database
        /// </summary>
        /// <param name="customer"></param>
        /// <returns></returns>
        public async Task<int> CreateCustomerAsync(CreateCustomerDto customer)
        {
            //For the sake of simplicity, the duplicate customer creation check
            //is not handled
            var newCustomer = new Customer
            {
                FirstName = customer.FirstName,
                LastName = customer.LastName,
                CreatedDate = DateTime.Now,
                IsActive = true
            };

            _customerContext.Customers.Add(newCustomer);

            //The Task.FromResult has been used instead of .SaveChangesAsync()
            //just to ensure that the unit tests can be written
            //however I prefer using .SaveChangesAsync()
            return await Task.FromResult(_customerContext.SaveChanges());
        }
        
        /// <summary>
        /// Updates the Customer data based on the customer dto
        /// inforation in database
        /// </summary>
        /// <param name="customer"></param>
        /// <returns></returns>
        public async Task UpdateCustomerAsync(CustomerDto customer)
        {
            var query = _customerContext.Customers.Where(x => x.Id == customer.Id);

            var customerInfo = await Task.FromResult(query.FirstOrDefault());

            if (customerInfo == null)
                throw new CustomException(string.Format(_CustomerErrorMessage, customer.Id));

            //For the sake of simplicity, the update resulting in duplicate
            //customer creation check is not handled
            customerInfo.FirstName = customer.FirstName;
            customerInfo.LastName = customer.LastName;
            customerInfo.UpdatedDate = DateTime.Now;

            //The Task.FromResult has been used instead of .SaveChangesAsync()
            //just to ensure that the unit tests can be written
            //however I prefer using .SaveChangesAsync()
            await Task.FromResult(_customerContext.SaveChanges());
        }

        /// <summary>
        /// Deletes the customer as per the given id
        /// from the database
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task DeleteCustomerAsync(int id)
        {
            var query = _customerContext.Customers
                .Where(x => x.Id == id  && x.IsActive.Value);

            var customerInfo = await Task.FromResult(query.FirstOrDefault());

            if (customerInfo == null)
                throw new CustomException(string.Format(_CustomerErrorMessage, id));

            customerInfo.IsActive = false;

            //The Task.FromResult has been used instead of .SaveChangesAsync()
            //just to ensure that the unit tests can be written
            //however I prefer using .SaveChangesAsync()
            await Task.FromResult(_customerContext.SaveChanges());
        }
    }
}
