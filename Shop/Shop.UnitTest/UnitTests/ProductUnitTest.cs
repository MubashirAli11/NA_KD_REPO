using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Shop.API.ApiModel.Response;
using Shop.API.Services;
using Shop.Core.CustomerRequestModels.RequestModels;
using Shop.Core.IServices;
using Shop.Core.RequestModels.ProductRequestModels;
using Shop.Core.ResponseModels.CustomerResponseModels;
using Shop.Infrastructure.ExternalServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Shop.UnitTest.UnitTests
{
    public class ProductUnitTest
    {
        [Fact]
        public async void Create_Product_Success_Test()
        {
            //Arrange

            var inMemoryCollection = GetInMemoryCollection();

            IConfiguration configuration = new ConfigurationBuilder()
                                .AddInMemoryCollection(inMemoryCollection).Build();

            IService httpService = new HttpService(configuration);

            IProductService<ApiResponse> productService = new ProductService(httpService, configuration);

            AddProductToWishListRequest addProductToWishListRequest= new AddProductToWishListRequest();
            addProductToWishListRequest.Name = $"Product{DateTime.Now.Ticks}";
            addProductToWishListRequest.Description = $"Description{DateTime.Now.Ticks}";
            addProductToWishListRequest.Id = Guid.NewGuid().ToString();

            var customerId = await CreateCustomer();

            //Act

            var response = await productService.Add(customerId, addProductToWishListRequest);

            //Assert

            Assert.True(response.IsSuccess);
            Assert.Equal(response.StatusCode, (int)System.Net.HttpStatusCode.Created);
            Assert.NotNull(response.Data);

            //This test should return product id as specified in the API documentation but it is 
            //returning no content
        }


        [Fact]
        public async void Delete_Product_Success_Test()
        {
            //Arrange

            var inMemoryCollection = GetInMemoryCollection();

            IConfiguration configuration = new ConfigurationBuilder()
                                .AddInMemoryCollection(inMemoryCollection).Build();

            IService httpService = new HttpService(configuration);

            IProductService<ApiResponse> productService = new ProductService(httpService, configuration);

            DeleteProductFromWishListRequest deleteProductFromWishListRequest = new DeleteProductFromWishListRequest();

            var customerId = await CreateCustomer();

            var productId = await AddProductToWishList(customerId);

            //Act

            var response = await productService.Delete(customerId, productId, deleteProductFromWishListRequest);

            //Assert

            Assert.True(response.IsSuccess);

            //This API should return success response but it is returning no content
        }


        #region private_function

        private async Task<string> AddProductToWishList(string customerId)
        {
            var inMemoryCollection = GetInMemoryCollection();

            IConfiguration configuration = new ConfigurationBuilder()
                                .AddInMemoryCollection(inMemoryCollection).Build();

            IService httpService = new HttpService(configuration);

            IProductService<ApiResponse> productService = new ProductService(httpService, configuration);
            ICustomerService<ApiResponse> customerService = new CustomerService(httpService, configuration);

            AddProductToWishListRequest addProductToWishListRequest = new AddProductToWishListRequest();
            addProductToWishListRequest.Name = $"Product{DateTime.Now.Ticks}";
            addProductToWishListRequest.Description = $"Description{DateTime.Now.Ticks}";
            addProductToWishListRequest.Id = Guid.NewGuid().ToString();


            var response = await productService.Add(customerId, addProductToWishListRequest);

            var customerWishList = await customerService.Get(customerId);

            var convertedResponse = JsonConvert.DeserializeObject<GetCustomerWishListResponse>
                                    (JsonConvert.SerializeObject(customerWishList.Data));

            return convertedResponse?.WishListProducts?.FirstOrDefault()?.Id ?? "";
        }


        private async Task<string> CreateCustomer()
        {
            var inMemoryCollection = GetInMemoryCollection();

            IConfiguration configuration = new ConfigurationBuilder()
                                .AddInMemoryCollection(inMemoryCollection).Build();

            IService httpService = new HttpService(configuration);

            ICustomerService<ApiResponse> customerService = new CustomerService(httpService, configuration);

            CreateCustomerRequest createCustomerRequest = new CreateCustomerRequest();
            createCustomerRequest.Name = $"Mubashir_{DateTime.Now.Ticks}";
            createCustomerRequest.Description = $"Description{DateTime.Now.Ticks}";
            createCustomerRequest.TenantId = Guid.NewGuid().ToString();
            createCustomerRequest.Enabled = true;

            var response = await customerService.Create(createCustomerRequest);

            return response.Data.ToString().Split("/")[4];
        }

        private IEnumerable<KeyValuePair<string, string>> GetInMemoryCollection()
        {
            List<KeyValuePair<string, string>> data = new List<KeyValuePair<string, string>>();

            data.Add(new KeyValuePair<string, string>("APIEndPoint:NAKD_BASE_SERVICE_API_URL", "https://nakdbaseserviceapi20211025120549.azurewebsites.net"));
            data.Add(new KeyValuePair<string, string>("APIEndPoint:CREATE_CUSTOMER_END_POINT", "api/customer/v1/customers"));
            data.Add(new KeyValuePair<string, string>("APIEndPoint:ADD_PRODUCT_TO_WISHLIST_END_POINT", "api/customer/v1/customers/{customer_id}/wishListProducts"));
            data.Add(new KeyValuePair<string, string>("APIEndPoint:DELETE_PRODUCT_FROM_WISHLIST_END_POINT", "api/customer/v1/customers/{customer_id}/wishListProducts/{product_id}"));
            data.Add(new KeyValuePair<string, string>("APIEndPoint:GET_CUSTOMER_WITH_WISHLIST_END_POINT", "api/customer/v1/customers/{customer_id}"));

            return data;
        }

        #endregion
    }
}
