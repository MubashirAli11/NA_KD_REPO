using Shop.Core.CustomerRequestModels.RequestModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop.Core.IServices
{
    public interface ICustomerService<T> where T : class
    {
        Task<T> Get(string customerId);
        Task<T> Create(CreateCustomerRequest createCustomerRequest);
    }
}
