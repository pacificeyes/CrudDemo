using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Data.Entity.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using BusinessEntities;
using Crud.Data.Models;
using Crud.Data.Repository;
using Crud.Data.Test.TestHelpers;

namespace Crud.Data.Test
{
    [TestClass]
    public class CustomerRepositoryTest
    {
        private Mock<CustomerContext> _mockContext;
        private CustomerRepository _customerRepository;

        public CustomerRepositoryTest()
        {
            _mockContext = CreateDbContext(false);
            _customerRepository = new CustomerRepository(_mockContext.Object);
        }

        private static IQueryable<Customer> GetFakeData(bool isEmpty)
        {
            if (isEmpty) return new List<Customer> { }.AsQueryable();
            var customers = new List<Customer> 
            { 
                new Customer 
                {
                    Id = 1,
                    FirstName = "FirstName1",
                    LastName = "LastName1",
                    CreatedDate = System.DateTime.Now,
                    UpdatedDate = null,
                    IsActive = true
                },
                new Customer
                {
                    Id = 2,
                    FirstName = "FirstName2",
                    LastName = "LastName2",
                    CreatedDate = System.DateTime.Now,
                    UpdatedDate = null,
                    IsActive = true
                }
            }.AsQueryable();

            return customers;
        }

        private Mock<CustomerContext> CreateDbContext(bool isEmpty)
        {
            var customers = GetFakeData(isEmpty);

            var mockSet = new Mock<DbSet<Customer>>();
            mockSet.As<IDbAsyncEnumerable<Customer>>()
                .Setup(m => m.GetAsyncEnumerator())
                .Returns(new TestDbAsyncEnumerator<Customer>(customers.GetEnumerator()));

            mockSet.As<IQueryable<Customer>>()
                .Setup(m => m.Provider)
                .Returns(new TestDbAsyncQueryProvider<Customer>(customers.Provider));

            mockSet.As<IQueryable<Customer>>().Setup(m => m.Expression).Returns(customers.Expression);
            mockSet.As<IQueryable<Customer>>().Setup(m => m.ElementType).Returns(customers.ElementType);
            mockSet.As<IQueryable<Customer>>().Setup(m => m.GetEnumerator()).Returns(customers.GetEnumerator());

            var context = new Mock<CustomerContext>();
            context.Setup(c => c.Customers).Returns(mockSet.Object);
            return context;
        }

        [TestMethod]
        public async Task Get_All_Customers_Async_Success_Test()
        {
            var customers = new List<CustomerDto>
            {
                new CustomerDto
                {
                    FirstName = "FirstName1",
                    Id = 1,
                    LastName = "LastName1"
                },
                new CustomerDto
                {
                    FirstName = "FirstName2",
                    Id = 2,
                    LastName = "LastName2"
                }
            };

            var output = await _customerRepository.GetAllCustomersAsync();

            Assert.AreEqual(2, output.Count);
            for (var i = 0; i < output.Count; i++)
            {
                Assert.AreEqual(customers[i].Id, output[i].Id);
                Assert.AreEqual(customers[i].FirstName, output[i].FirstName);
                Assert.AreEqual(customers[i].LastName, output[i].LastName);
            }
        }

        [TestMethod]
        public async Task Get_All_Customers_Async_No_Data_Returned_Test()
        {
            _mockContext = CreateDbContext(true);
            _customerRepository = new CustomerRepository(_mockContext.Object);
            var i = 0;
            try
            {
                var output = await _customerRepository.GetAllCustomersAsync();
            }
            catch (CustomException ex)
            {
                i++;
                Assert.AreEqual(ex.Message, "There are no customers available to display");
            }

            Assert.AreEqual(1, i);
        }

        [TestMethod]
        public async Task Get_Customer_Async_Success_Test()
        {
            _mockContext = CreateDbContext(false);
            _customerRepository = new CustomerRepository(_mockContext.Object);
            
            var output = await _customerRepository.GetCustomerAsync(1);
            Assert.AreEqual(1, output.Id);
            Assert.AreEqual("FirstName1", output.FirstName);
            Assert.AreEqual("LastName1", output.LastName);
        }

        [TestMethod]
        public async Task Get_Customer_Async_No_record_Found_In_Db_Test()
        {
            _mockContext = CreateDbContext(true);
            _customerRepository = new CustomerRepository(_mockContext.Object);
            var i = 0;
            try
            {
                var output = await _customerRepository.GetCustomerAsync(1);
            }
            catch (CustomException ex)
            {
                i++;
                Assert.AreEqual(ex.Message, "The customer with Id - 1 does not exist");
            }

            Assert.AreEqual(1, i);
        }

        [TestMethod]
        public async Task Create_Customer_Async_Success_Test()
        {
            _mockContext = CreateDbContext(true);
            _customerRepository = new CustomerRepository(_mockContext.Object);
            var i = 0;
            _mockContext.Setup(c => c.SaveChanges()).Returns(() => i++).Verifiable();
            var customerDto = new CreateCustomerDto
            {
                FirstName = "FirstName11",
                LastName = "LastName11"
            };

            var output = await _customerRepository.CreateCustomerAsync(customerDto);
            _mockContext.Verify();
            Assert.AreEqual(1, i);
        }

        [TestMethod]
        public async Task Update_Customer_Async_Success_Test()
        {
            _mockContext = CreateDbContext(false);
            _customerRepository = new CustomerRepository(_mockContext.Object);
            var i = 0;
            _mockContext.Setup(c => c.SaveChanges()).Returns(() => i++).Verifiable();
            var customerDto = new CustomerDto
            {
                Id = 2, 
                FirstName = "FirstName22",
                LastName = "LastName22"
            };

            await _customerRepository.UpdateCustomerAsync(customerDto);
            var output = await _customerRepository.GetCustomerAsync(2);
            _mockContext.Verify();
            Assert.AreEqual(1, i);
            Assert.AreEqual(customerDto.Id, output.Id);
            Assert.AreEqual(customerDto.FirstName, output.FirstName);
            Assert.AreEqual(customerDto.LastName, output.LastName);
        }

        [TestMethod]
        public async Task Update_Customer_Async_Non_Existing_Record_Test()
        {
            _mockContext = CreateDbContext(false);
            _customerRepository = new CustomerRepository(_mockContext.Object);
            var i = 0;
            
            var customerDto = new CustomerDto
            {
                Id = 5,
                FirstName = "FirstName5",
                LastName = "LastName5"
            };

            try
            {
                await _customerRepository.UpdateCustomerAsync(customerDto);
            }
            catch(CustomException ex)
            {
                Assert.AreEqual($"The customer with Id - {customerDto.Id} does not exist", ex.Message);
                i++;
            }
            
            _mockContext.Verify();
            Assert.AreEqual(1, i);
        }

        [TestMethod]
        public async Task Delete_Customer_Async_Success_Test()
        {
            _mockContext = CreateDbContext(false);
            _customerRepository = new CustomerRepository(_mockContext.Object);
            int i = 0, exCounter = 0;
            _mockContext.Setup(c => c.SaveChanges()).Returns(() => i++).Verifiable();
            const int customerId = 1;

            await _customerRepository.DeleteCustomerAsync(customerId);
            try
            {
                var output = await _customerRepository.GetCustomerAsync(1);
            }
            catch(CustomException ex)
            {
                Assert.AreEqual($"The customer with Id - {customerId} does not exist", ex.Message);
                exCounter++;
            }
            _mockContext.Verify();
            Assert.AreEqual(1, i);
            Assert.AreEqual(1, exCounter);
        }

        [TestMethod]
        public async Task Delete_Customer_Async_Non_Existing_Record_Test()
        {
            _mockContext = CreateDbContext(false);
            _customerRepository = new CustomerRepository(_mockContext.Object);
            var i = 0;
            const int customerId = 5;

            try
            {
                await _customerRepository.DeleteCustomerAsync(customerId);
            }
            catch (CustomException ex)
            {
                Assert.AreEqual($"The customer with Id - {customerId} does not exist", ex.Message);
                i++;
            }

            Assert.AreEqual(1, i);
        }
    }
}
