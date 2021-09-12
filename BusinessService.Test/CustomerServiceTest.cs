using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using BusinessEntities;
using Crud.Data.Interface;

namespace BusinessService.Test
{
    [TestClass]
    public class CustomerServiceTest
    {
        private readonly CustomerService _customerService;
        private readonly Mock<ICustomerRepository> _customerRepoMock;
        private List<CustomerDto> _customerList;

        public CustomerServiceTest()
        {
            _customerRepoMock = new Mock<ICustomerRepository>();
            _customerService = new CustomerService(_customerRepoMock.Object);
            _customerList = new List<CustomerDto>
            {
                new CustomerDto
                {
                    Id = 1,
                    FirstName = "Daniel",
                    LastName = "Cruz"
                },
                new CustomerDto
                {
                    Id = 2,
                    FirstName = "Tony",
                    LastName = "Neilson"
                },
                new CustomerDto
                {
                    Id = 3,
                    FirstName = "Harry",
                    LastName = "Morrison"
                }
            };
        }

        [TestMethod]
        public async Task Get_All_Customers_Success_Test()
        {
            _customerRepoMock.Setup(x => x.GetAllCustomersAsync()).Returns(Task.FromResult(_customerList));

            var output = await _customerService.GetAllCustomersAsync();
            CollectionAssert.AreEqual(_customerList, output);
        }

        [TestMethod]
        public async Task Get_Customer_Success_Test()
        {
            var customer = new CustomerDto
            {
                Id = 1,
                FirstName = "Daniel",
                LastName = "Cruz"
            };

            const int customerId = 1;
            _customerRepoMock.Setup(x => x.GetCustomerAsync(It.IsAny<int>())).Returns(Task.FromResult(customer));

            var output = await _customerService.GetCustomerAsync(customerId);
            Assert.AreEqual(customer.Id, output.Id);
            Assert.AreEqual(customer.FirstName, output.FirstName);
            Assert.AreEqual(customer.LastName, output.LastName);
        }

        [TestMethod]
        public async Task Create_Customer_Success_Test()
        {
            var customer = new CreateCustomerDto
            {
                FirstName = "Daniel",
                LastName = "Cruz"
            };

            _customerRepoMock.Setup(x => x.CreateCustomerAsync(It.IsAny<CreateCustomerDto>())).Returns(Task.FromResult(4));

            var output = await _customerService.CreateCustomerAsync(customer);
            Assert.AreEqual(4, output);
        }

        [TestMethod]
        public async Task Delete_Customer_Success_Test()
        {
            const int customerId = 1;
            var i = 0;
            _customerRepoMock.Setup(x => x.DeleteCustomerAsync(It.IsAny<int>())).Callback(() => i++)
                .Returns(Task.CompletedTask);
            await _customerService.DeleteCustomerAsync(customerId);
            Assert.IsTrue(i == 1);
        }

        [TestMethod]
        public async Task Update_Customer_Success_Test()
        {
            var customer = new CustomerDto
            {
                Id = 1,
                FirstName = "Daniel",
                LastName = "Anderson"
            };

            var i = 0;
            _customerRepoMock.Setup(x => x.UpdateCustomerAsync(customer)).Callback(() => i++)
                .Returns(Task.CompletedTask);

            await _customerService.UpdateCustomerAsync(customer);
            Assert.IsTrue(i == 1);
        }
    }
}
