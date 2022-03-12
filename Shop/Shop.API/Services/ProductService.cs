using Shop.API.ApiModel.Response;
using Shop.Core.IServices;
using Shop.Core.RequestModels.ProductRequestModels;

namespace Shop.API.Services
{
    public class ProductService : IProductService<ApiResponse>
    {
        private readonly IService _service;
        private readonly string _addProductToWishListEndPoint;
        public ProductService(IService service, IConfiguration configuration)
        {
            _service = service;
            _addProductToWishListEndPoint = configuration["APIEndPoint:ADD_PRODUCT_TO_WISHLIST_END_POINT"];
        }

        public async Task<ApiResponse> Add(string customerId, AddProductToWishListRequest addProductToWishListRequest)
        {
            try
            {
                addProductToWishListRequest.CustomerId = customerId;
                var response = await _service.AddProductToWishList(addProductToWishListRequest);

                if (response.Item1 != null && String.IsNullOrEmpty(response.Item1.Id))
                    return ApiResponse.ReturnCreatedResponse("Product added to wish list", $"{_addProductToWishListEndPoint}/{response.Item1.Id}");

                else if (response.Item2 == (int)System.Net.HttpStatusCode.Unauthorized)
                    return ApiResponse.CreateUnauthorizeResponse();

                else if (response.Item2 == (int)System.Net.HttpStatusCode.NoContent)
                    return ApiResponse.CreateNoContentResponse();

                else if (response.Item2 == (int)System.Net.HttpStatusCode.Forbidden)
                    return ApiResponse.CreateForbiddenResponse();

                else if (response.Item2 == (int)System.Net.HttpStatusCode.NotFound)
                    return ApiResponse.CreateNotFoundResponse();

                else
                    return ApiResponse.CreateFailedResponse("Something went wrong!");


            }
            catch (Exception exp)
            {
                return ApiResponse.CreateFailedResponse(exp.Message);
            }
        }

        public async Task<ApiResponse> Delete(string customerId, string productId, DeleteProductFromWishListRequest deleteProductFromWishListRequest)
        {
            try
            {
                deleteProductFromWishListRequest.ProductId = productId;
                deleteProductFromWishListRequest.Id = customerId;
                var response = await _service.DeleteProductFromWishList(deleteProductFromWishListRequest);

                if (response.Item1 != null && response.Item2 == (int)System.Net.HttpStatusCode.OK)
                    return ApiResponse.CreateSuccessResponse("Product deleted from wish list", response.Item1);

                else if (response.Item2 == (int)System.Net.HttpStatusCode.Unauthorized)
                    return ApiResponse.CreateUnauthorizeResponse();

                else if (response.Item2 == (int)System.Net.HttpStatusCode.NoContent)
                    return ApiResponse.CreateNoContentResponse();

                else if (response.Item2 == (int)System.Net.HttpStatusCode.Forbidden)
                    return ApiResponse.CreateForbiddenResponse();

                else if (response.Item2 == (int)System.Net.HttpStatusCode.NotFound)
                    return ApiResponse.CreateNotFoundResponse();

                else
                    return ApiResponse.CreateFailedResponse("Something went wrong!");


            }
            catch (Exception exp)
            {
                return ApiResponse.CreateFailedResponse(exp.Message);
            }
        }
    }
}
