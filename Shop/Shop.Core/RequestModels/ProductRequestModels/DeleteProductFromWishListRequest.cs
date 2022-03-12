using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop.Core.RequestModels.ProductRequestModels
{
    public class DeleteProductFromWishListRequest
    {
        public string Id { get; set; }
        public string ProductId { get; set; }
    }
}
