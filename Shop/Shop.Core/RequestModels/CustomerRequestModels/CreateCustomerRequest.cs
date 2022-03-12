using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop.Core.CustomerRequestModels.RequestModels
{
    public class CreateCustomerRequest
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string TenantId { get; set; }
        public bool Enabled { get; set; }
    }
}
