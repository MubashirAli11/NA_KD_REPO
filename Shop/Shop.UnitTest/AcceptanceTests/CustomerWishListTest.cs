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

namespace Shop.UnitTest.AcceptanceTests
{
    public class CustomerWishListTest
    {
        /// <summary>
        /// Create a customer
        /// Add product to customer wishlist so that I will be able to check customer wishlist
        /// Delete a product from customer wishlist so that I can check there is no product in customer wish list
        /// </summary>
        [Fact]
        public async void CustomerWishListAcceptanceTest()
        {

            var customerId = await CreateCustomer();
            
            //Check if customer is created successfully
            Assert.NotNull(customerId);
            Assert.NotEmpty(customerId);



            var productId = Guid.NewGuid().ToString();

            //Check if product is created successfully
            Assert.NotNull(productId);
            Assert.NotEmpty(productId);

            await AddProductToWishList(customerId, productId);

            //Check if product is added in customer wishlist
            Assert.True(await ProductExistInCustomerWishList(customerId, productId));

            await DeleteProductFromWishList(customerId, productId);

            //Check if product is deleted from customer wishlist
            Assert.False(await ProductExistInCustomerWishList(customerId, productId));

        }

        private async Task<bool> ProductExistInCustomerWishList(string customerId, string productId)
        {
            var inMemoryCollection = GetInMemoryCollection();

            IConfiguration configuration = new ConfigurationBuilder()
                                .AddInMemoryCollection(inMemoryCollection).Build();

            IService httpService = new HttpService(configuration);

            ICustomerService<ApiResponse> customerService = new CustomerService(httpService, configuration);

            var customerWishList = await customerService.Get(customerId);

            var convertedResponse = JsonConvert.DeserializeObject<GetCustomerWishListResponse>
                                    (JsonConvert.SerializeObject(customerWishList.Data));

            return convertedResponse.WishListProducts.Any(x => x.Id == productId);
        }

        private async Task DeleteProductFromWishList(string customerId, string productId)
        {
            var inMemoryCollection = GetInMemoryCollection();

            IConfiguration configuration = new ConfigurationBuilder()
                                .AddInMemoryCollection(inMemoryCollection).Build();

            IService httpService = new HttpService(configuration);

            IProductService<ApiResponse> productService = new ProductService(httpService, configuration);

            DeleteProductFromWishListRequest deleteProductFromWishListRequest = new DeleteProductFromWishListRequest();

            var response = await productService.Delete(customerId, productId, deleteProductFromWishListRequest);

        }


        #region private_function

        private async Task AddProductToWishList(string customerId, string productId)
        {
            var inMemoryCollection = GetInMemoryCollection();

            IConfiguration configuration = new ConfigurationBuilder()
                                .AddInMemoryCollection(inMemoryCollection).Build();

            IService httpService = new HttpService(configuration);

            IProductService<ApiResponse> productService = new ProductService(httpService, configuration);

            AddProductToWishListRequest addProductToWishListRequest = new AddProductToWishListRequest();
            addProductToWishListRequest.Name = $"Product{DateTime.Now.Ticks}";
            addProductToWishListRequest.Description = $"Description{DateTime.Now.Ticks}";
            addProductToWishListRequest.Id = productId;


            var response = await productService.Add(customerId, addProductToWishListRequest);
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
