using Eshop.Api.Models.Contacts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eshop.Api.BusinessLayer.Services.Contacts
{
    public interface ICustomerService
    {
        public Customer GetCustomer(int customerId);
        public Customer GetCustomerByUserId(string userId);
        public Customer GetCustomerByToken(string token);

        public bool CreateCustomer(Customer customer);
    }
}
