using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Shop.API.ApiModel.Response;
using Shop.Core.IServices;
using Shop.Core.RequestModels.ProductRequestModels;
using System.Net;

namespace Shop.API.Controllers
{
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    [ApiVersion("1.0")]
    public class ProductController : ControllerBase
    {
        private readonly IProductService<ApiResponse> _productService;

        public ProductController(
          IProductService<ApiResponse> productService)
        {
            _productService = productService;
        }

        [HttpPost("{customerId}")]
        [ProducesResponseType((int)HttpStatusCode.Created)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> Post(string customerId, [FromBody] AddProductToWishListRequest addProductToWishListRequest)
        {

            var response = await _productService.Add(customerId, addProductToWishListRequest);

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


            return response.IsSuccess ? Created(response.Data.ToString(), response) : BadRequest() as IActionResult;

        }


        [HttpDelete("{customerId}/{productId}")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> Delete(string customerId, string productId, [FromBody] DeleteProductFromWishListRequest deleteProductFromWishListRequest)
        {

            var response = await _productService.Delete(customerId, productId, deleteProductFromWishListRequest);

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

            return response.IsSuccess ? Ok(response) : BadRequest() as IActionResult;
        }

    }
}
