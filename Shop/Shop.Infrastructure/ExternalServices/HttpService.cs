using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using RestSharp;
using Shop.Core.CustomerRequestModels.RequestModels;
using Shop.Core.IServices;
using Shop.Core.RequestModels.ProductRequestModels;
using Shop.Core.ResponseModels;
using Shop.Core.ResponseModels.CustomerResponseModels;
using Shop.Core.ResponseModels.ErrorResponseModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop.Infrastructure.ExternalServices
{
    public class HttpService : IService
    {
        private readonly RestClient _httpClient;
        private readonly string _createCustomerEndPoint;
        private readonly string _addProductToWishListEndPoint;
        private readonly string _getCustomerWithWishListEndPoint;
        private readonly string _deleteProductFromWishListEndPoint;
        public HttpService(IConfiguration configuration)
        {
            _httpClient = new RestClient(configuration["APIEndPoint:NAKD_BASE_SERVICE_API_URL"]);
            _createCustomerEndPoint = configuration["APIEndPoint:CREATE_CUSTOMER_END_POINT"];
            _addProductToWishListEndPoint = configuration["APIEndPoint:ADD_PRODUCT_TO_WISHLIST_END_POINT"];
            _getCustomerWithWishListEndPoint = configuration["APIEndPoint:GET_CUSTOMER_WITH_WISHLIST_END_POINT"];
            _deleteProductFromWishListEndPoint = configuration["APIEndPoint:DELETE_PRODUCT_FROM_WISHLIST_END_POINT"];
        }


        public async Task<(ResourceCreatedResponse, int)> CreateCustomer(CreateCustomerRequest createCustomerRequest)
        {
            var httpRequest = new RestRequest(_createCustomerEndPoint, Method.Post);
            httpRequest.AddBody(createCustomerRequest);

            var response = await _httpClient.ExecuteAsync(httpRequest);

            if (response == null)
                return (null, (int)System.Net.HttpStatusCode.NoContent);

            if (response.StatusCode == System.Net.HttpStatusCode.Created && !String.IsNullOrEmpty(response.Content))
                return (JsonConvert.DeserializeObject<ResourceCreatedResponse>(response.Content), (int)response.StatusCode);

            else if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
            {
                var errorResponse = JsonConvert.DeserializeObject<ErrorResponse>(response.Content);
                throw new Exception(String.Join(", ", errorResponse.Errors.Body.ToList()));
            }
            else
                return (null, (int)response.StatusCode);
        }

        public async Task<(ResourceCreatedResponse, int)> AddProductToWishList(AddProductToWishListRequest addProductToWishListRequest)
        {
            var httpRequest = new RestRequest(_addProductToWishListEndPoint.Replace("{customer_id}", addProductToWishListRequest.CustomerId), Method.Post);
            httpRequest.AddBody(addProductToWishListRequest);

            var response = await _httpClient.ExecuteAsync(httpRequest);

            if (response == null)
                return (null, (int)System.Net.HttpStatusCode.NoContent);

            if (response.StatusCode == System.Net.HttpStatusCode.Created && !String.IsNullOrEmpty(response.Content))
                return (JsonConvert.DeserializeObject<ResourceCreatedResponse>(response.Content), (int)response.StatusCode);

            else if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
            {
                var errorResponse = JsonConvert.DeserializeObject<ErrorResponse>(response.Content);
                throw new Exception(String.Join(", ", errorResponse.Errors.Body.ToList()));
            }
            else
                return (null, (int)response.StatusCode);

        }

        public async Task<(bool, int)> DeleteProductFromWishList(DeleteProductFromWishListRequest deleteProductFromWishList)
        {
            var httpRequest = new RestRequest(_deleteProductFromWishListEndPoint
                                              .Replace("{customer_id}", deleteProductFromWishList.Id)
                                              .Replace("{product_id}", deleteProductFromWishList.ProductId), Method.Delete);
            httpRequest.AddBody(deleteProductFromWishList);

            var response = await _httpClient.ExecuteAsync(httpRequest);

            if (response == null)
                return (false, (int)System.Net.HttpStatusCode.NoContent);

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
                return (true, (int)response.StatusCode);

            else if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
            {
                var errorResponse = JsonConvert.DeserializeObject<ErrorResponse>(response.Content);
                throw new Exception(String.Join(", ", errorResponse.Errors.Body.ToList()));
            }
            else
                return (false, (int)response.StatusCode);
        }

        public async Task<(GetCustomerWishListResponse, int)> GetCustomerWishList(string customerId)
        {
            var httpRequest = new RestRequest(_getCustomerWithWishListEndPoint.Replace("{customer_id}", customerId), Method.Get);

            var response = await _httpClient.ExecuteAsync(httpRequest);

            if (response == null)
                return (null, (int)System.Net.HttpStatusCode.NoContent);

            if (response.StatusCode == System.Net.HttpStatusCode.OK && !String.IsNullOrEmpty(response.Content))
                return (JsonConvert.DeserializeObject<GetCustomerWishListResponse>(response.Content), (int)response.StatusCode);

            else if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
            {
                var errorResponse = JsonConvert.DeserializeObject<ErrorResponse>(response.Content);
                throw new Exception(String.Join(", ", errorResponse.Errors.Body.ToList()));
            }
            else
                return (null, (int)response.StatusCode);
        }
    }
}
