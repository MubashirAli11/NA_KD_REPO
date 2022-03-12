﻿using Microsoft.Extensions.Configuration;
using Shop.API.ApiModel.Response;
using Shop.API.Services;
using Shop.Core.CustomerRequestModels.RequestModels;
using Shop.Core.IServices;
using Shop.Infrastructure.ExternalServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Shop.UnitTest.UnitTests
{
    public class CustomerUnitTest
    {
        [Fact]
        public async void Create_Customer_Success_Test()
        {
            //Arrange

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

            //Act

            var response = await customerService.Create(createCustomerRequest);

            //Assert
            
            Assert.True(response.IsSuccess);
            Assert.Equal(response.StatusCode, (int)System.Net.HttpStatusCode.Created);
            Assert.NotNull(response.Data);
        }


        #region private_functions

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
