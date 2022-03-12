using Shop.Core.RequestModels.ProductRequestModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop.Core.IServices
{
    public interface IProductService<T> where T : class
    {
        Task<T> Add(string customerId, AddProductToWishListRequest addProductToWishListRequest);
        Task<T> Delete(string customerId, string productId, DeleteProductFromWishListRequest deleteProductFromWishListRequest);
    }
}
