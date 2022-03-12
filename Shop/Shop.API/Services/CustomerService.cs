using Shop.API.ApiModel.Response;
using Shop.Core.CustomerRequestModels.RequestModels;
using Shop.Core.IServices;

namespace Shop.API.Services
{
    public class CustomerService : ICustomerService<ApiResponse>
    {
        private readonly IService _service;
        private readonly string _getCustomerWithWishListEndPoint;
        public CustomerService(IService service, IConfiguration configuration)
        {
            _service = service;
            _getCustomerWithWishListEndPoint = configuration["APIEndPoint:GET_CUSTOMER_WITH_WISHLIST_END_POINT"];
        }

        public async Task<ApiResponse> Create(CreateCustomerRequest createCustomerRequest)
        {
            try
            {
                var response = await _service.CreateCustomer(createCustomerRequest);

                if (response.Item1 != null && !String.IsNullOrEmpty(response.Item1.Id))
                    return ApiResponse.ReturnCreatedResponse("Customer created", $"{_getCustomerWithWishListEndPoint}/{response.Item1.Id}");

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

        public async Task<ApiResponse> Get(string customerId)
        {
            try
            {
                var response = await _service.GetCustomerWishList(customerId);

                if (response.Item1 != null)
                    return ApiResponse.CreateSuccessResponse("Success", response.Item1);

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
