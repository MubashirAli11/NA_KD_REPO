using Shop.Core.CustomerRequestModels.RequestModels;
using Shop.Core.RequestModels.ProductRequestModels;
using Shop.Core.ResponseModels;
using Shop.Core.ResponseModels.CustomerResponseModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop.Core.IServices
{
    public interface IService
    {
        Task<(ResourceCreatedResponse, int)> CreateCustomer(CreateCustomerRequest createCustomerRequest);
        Task<(ResourceCreatedResponse, int)> AddProductToWishList(AddProductToWishListRequest addProductToWishListRequest);
        Task<(GetCustomerWishListResponse, int)> GetCustomerWishList(string customerId);
        Task<(bool, int)> DeleteProductFromWishList(DeleteProductFromWishListRequest deleteProductFromWishList);
    }
}
