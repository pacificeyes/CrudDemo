using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using CrudApi.Controllers;
using BusinessService;
using BusinessEntities;

namespace CrudApi.Test
{
    [TestClass]
    public class CustomerControllerTest
    {
        private readonly CustomerController _customerController;
        private readonly Mock<ICustomerService> _customerServiceMock;
        public CustomerControllerTest() 
        {
            _customerServiceMock = new Mock<ICustomerService>();
            _customerController = new CustomerController(_customerServiceMock.Object);
        }

        [TestMethod]
        public async Task Get_All_Customers_Api_Success_Test()
        {
            var customerList = new List<CustomerDto>
            {
                new CustomerDto
                {
                    Id = 1,
                    FirstName = "Customer1",
                    LastName = "Surname1"
                },
                new CustomerDto
                {
                    Id = 2,
                    FirstName = "Customer2",
                    LastName = "Surname2"
                },
                new CustomerDto
                {
                    Id = 1,
                    FirstName = "Customer3",
                    LastName = "Surname3"
                }
            };

            _customerServiceMock.Setup(x => x.GetAllCustomersAsync()).Returns(Task.FromResult(customerList));

            var contentResult = await _customerController.Get() as OkObjectResult;
            CollectionAssert.AreEqual(customerList, (System.Collections.ICollection)contentResult.Value);
        }

        [TestMethod]
        public async Task Get_All_Customers_Api_Custom_Exception_Test()
        {
            var ex = new CustomException("No customer data available to display");
            _customerServiceMock.Setup(x => x.GetAllCustomersAsync()).Throws(ex);

            var actionResult = await _customerController.Get() as NotFoundObjectResult;
            
            Assert.AreEqual(404, actionResult.StatusCode);
            Assert.AreEqual(ex.Message, actionResult.Value);
        }

        [TestMethod]
        public async Task Get_All_Customers_Api_Exception_Test()
        {
            var ex = new Exception("Error Occurred");
            _customerServiceMock.Setup(x => x.GetAllCustomersAsync()).Throws(ex);

            var actionResult = await _customerController.Get() as ObjectResult;

            Assert.AreEqual(500, actionResult.StatusCode);
            Assert.AreEqual(ex.Message, actionResult.Value);
        }

        [TestMethod]
        public async Task Get_Customer_Api_Success_Test()
        {
            var customer = new CustomerDto
            {
                Id = 1,
                FirstName = "Customer1",
                LastName = "Surname1"
            };

            const int Id = 1;

            _customerServiceMock.Setup(x => x.GetCustomerAsync(It.IsAny<int>())).Returns(Task.FromResult(customer));

            var okResult = await _customerController.Get(Id);
            Assert.IsInstanceOfType(okResult, typeof(OkObjectResult));
        }

        [TestMethod]
        public async Task Get_Customer_Api_Custom_Exception_Test()
        {
            var ex = new CustomException("No customer data available to display");
            const int Id = 1;

            _customerServiceMock.Setup(x => x.GetCustomerAsync(It.IsAny<int>())).Throws(ex);

            var actionResult = await _customerController.Get(Id) as NotFoundObjectResult;

            Assert.AreEqual(ex.Message, actionResult.Value);
            Assert.AreEqual(404, actionResult.StatusCode);
        }

        [TestMethod]
        public async Task Get_Customer_Api_Exception_Test()
        {
            var ex = new Exception("Error Occurred");
            const int Id = 1;
            _customerServiceMock.Setup(x => x.GetCustomerAsync(It.IsAny<int>())).Throws(ex);

            var actionResult = await _customerController.Get(Id) as ObjectResult;

            Assert.AreEqual(500, actionResult.StatusCode);
            Assert.AreEqual(ex.Message, actionResult.Value.ToString());
        }

        [TestMethod]
        public async Task Post_Customer_Api_Success_Test()
        {
            var customer = new CreateCustomerDto
            {
                FirstName = "Customer5",
                LastName = "Surname5"
            };

            _customerServiceMock.Setup(x => x.CreateCustomerAsync(It.IsAny<CreateCustomerDto>())).Returns(Task.FromResult(1));

            var okResult = await _customerController.Post(customer) as OkObjectResult;
            Assert.AreEqual(200, okResult.StatusCode);
        }

        [TestMethod]
        public async Task Post_Customer_Api_Exception_Test()
        {
            var ex = new Exception("Error Occurred");
            var newCustomer = new CreateCustomerDto
            {
                FirstName = "User 4",
                LastName = "Surname 4"
            };

            _customerServiceMock.Setup(x => x.CreateCustomerAsync(It.IsAny<CreateCustomerDto>())).Throws(ex);

            var actionResult = await _customerController.Post(newCustomer) as ObjectResult;

            Assert.AreEqual(500, actionResult.StatusCode);
            Assert.AreEqual(ex.Message, actionResult.Value.ToString());
        }

        [TestMethod]
        public async Task Put_Customer_Api_Success_Test()
        {
            var customer = new CustomerDto
            {
                Id = 4,
                FirstName = "User 4",
                LastName = "Surname 4"
            };

            _customerServiceMock.Setup(x => x.UpdateCustomerAsync(It.IsAny<CustomerDto>())).Returns(Task.CompletedTask);

            var okResult = await _customerController.Post(customer) as OkObjectResult;
            Assert.AreEqual(200, okResult.StatusCode);
        }

        [TestMethod]
        public async Task Put_Customer_Api_Custom_Exception_Test()
        {
            var ex = new CustomException("Customer record not found");
            var customer = new CustomerDto
            {
                Id = 4,
                FirstName = "User 4",
                LastName = "Surname 4"
            };

            _customerServiceMock.Setup(x => x.UpdateCustomerAsync(It.IsAny<CustomerDto>())).Throws(ex);

            var actionResult = await _customerController.Put(customer) as NotFoundObjectResult;

            Assert.AreEqual(ex.Message, actionResult.Value);
            Assert.AreEqual(404, actionResult.StatusCode);
        }

        [TestMethod]
        public async Task Put_Customer_Api_Exception_Test()
        {
            var ex = new Exception("Error Occurred");
            var customer = new CustomerDto
            {
                Id = 4,
                FirstName = "User 4",
                LastName = "Surname 4"
            };

            _customerServiceMock.Setup(x => x.UpdateCustomerAsync(It.IsAny<CustomerDto>())).Throws(ex);

            var actionResult = await _customerController.Put(customer) as ObjectResult;

            Assert.AreEqual(500, actionResult.StatusCode);
            Assert.AreEqual(ex.Message, actionResult.Value.ToString());
        }

        [TestMethod]
        public async Task Delete_Customer_Api_Success_Test()
        {
            const int customerId = 4;

            _customerServiceMock.Setup(x => x.DeleteCustomerAsync(It.IsAny<int>())).Returns(Task.CompletedTask);

            var okResult = await _customerController.Delete(customerId) as OkObjectResult;
            Assert.AreEqual(200, okResult.StatusCode);
        }

        [TestMethod]
        public async Task Delete_Customer_Api_Custom_Exception_Test()
        {
            const int customerId = 4;
            var ex = new CustomException($"Customer with Id - {customerId} not found"); 

            _customerServiceMock.Setup(x => x.DeleteCustomerAsync(It.IsAny<int>())).Throws(ex);

            var actionResult = await _customerController.Delete(customerId) as NotFoundObjectResult;

            Assert.AreEqual(ex.Message, actionResult.Value);
            Assert.AreEqual(404, actionResult.StatusCode);
        }

        [TestMethod]
        public async Task Delete_Customer_Api_Exception_Test()
        {
            const int customerId = 4;
            var ex = new Exception("Error Occurred");
            
            _customerServiceMock.Setup(x => x.DeleteCustomerAsync(It.IsAny<int>())).Throws(ex);

            var actionResult = await _customerController.Delete(customerId) as ObjectResult;

            Assert.AreEqual(500, actionResult.StatusCode);
            Assert.AreEqual(ex.Message, actionResult.Value.ToString());
        }
    }
}
