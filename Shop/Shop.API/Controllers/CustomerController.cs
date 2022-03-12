using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Shop.API.ApiModel.Response;
using Shop.Core.CustomerRequestModels.RequestModels;
using Shop.Core.IServices;
using System.Net;

namespace Shop.API.Controllers
{
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    [ApiVersion("1.0")]
    public class CustomerController : ControllerBase
    {

        private readonly ICustomerService<ApiResponse> _customerService;

        public CustomerController(
            ICustomerService<ApiResponse> customerService)
        {
            _customerService = customerService;
        }


        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.Created)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> Post([FromBody] CreateCustomerRequest createCustomerRequest)
        {
            var response = await _customerService.Create(createCustomerRequest);

            if (response == null)
                return NoContent();

            else if (response.StatusCode == (int)System.Net.HttpStatusCode.NoContent)
                return NoContent();

            else if (response.StatusCode == (int)System.Net.HttpStatusCode.Unauthorized)
                return Unauthorized();

            else if (response.StatusCode == (int)System.Net.HttpStatusCode.Forbidden)
                return Forbid();

            else if (response.StatusCode == (int)System.Net.HttpStatusCode.NotFound)
                return NotFound();

            return response.IsSuccess ? Created(response.Data.ToString(), response) : BadRequest(response) as IActionResult;

        }


        [HttpGet("{id}")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> Get(string id)
        {

            var response = await _customerService.Get(id);

            if (response == null)
                return NoContent();

            else if (response.StatusCode == (int)System.Net.HttpStatusCode.NoContent)
                return NoContent();

            else if (response.StatusCode == (int)System.Net.HttpStatusCode.Unauthorized)
                return Unauthorized();

            else if (response.StatusCode == (int)System.Net.HttpStatusCode.Forbidden)
                return Forbid();

            else if (response.StatusCode == (int)System.Net.HttpStatusCode.NotFound)
                return NotFound();

            return response.IsSuccess ? Ok(response) : BadRequest(response) as IActionResult;
        }
    }
}
