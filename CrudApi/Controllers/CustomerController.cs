using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using BusinessEntities;
using BusinessService;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CrudApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly ICustomerService _customerService;
        public CustomerController(ICustomerService customerService)
        {
            _customerService = customerService;
        }

        // GET: api/<CustomerController>
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                return Ok(await _customerService.GetAllCustomersAsync());
            }
            catch (CustomException ex)
            {
                //Log the exception
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                //Log the exception
                var result = StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
                return result;
            }
        }

        // GET api/<CustomerController>/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            try
            {
                return Ok(await _customerService.GetCustomerAsync(id));
            }
            catch(CustomException ex)
            {
                //Log the exception
                return NotFound(ex.Message);
            }
            catch(Exception ex)
            {
                //Log the exception
                var result = StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
                return result;
            }
        }

        // POST api/<CustomerController>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] CreateCustomerDto customerDto)
        {
            try
            {
                return Ok(await _customerService.CreateCustomerAsync(customerDto));
            }
            catch (Exception ex)
            {
                //Log the exception
                var result = StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
                return result;
            }
        }

        // PUT api/<CustomerController>
        [HttpPut]
        public async Task<IActionResult> Put([FromBody] CustomerDto customerDto)
        {
            try
            {
                await _customerService.UpdateCustomerAsync(customerDto);
                return Ok("Success");
            }
            catch(CustomException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                //Log the exception
                var result = StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
                return result;
            }
        }

        // DELETE api/<CustomerController>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _customerService.DeleteCustomerAsync(id);
                return Ok("Success");
            }
            catch (CustomException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                //Log the exception
                var result = StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
                return result;
            }
        }
    }
}
