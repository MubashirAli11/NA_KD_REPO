using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop.Core.RequestModels.ProductRequestModels
{
    public class AddProductToWishListRequest
    {
        public string? CustomerId { get; set; }
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
