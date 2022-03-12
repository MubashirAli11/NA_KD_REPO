using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop.Core.ResponseModels.CustomerResponseModels
{
    public class GetCustomerWishListResponse
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool Enabled { get; set; }
        public List<CustomerWishList> WishListProducts { get; set; }
    }
}
