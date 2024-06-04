using Eshop.Api.Models.Contacts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eshop.Api.DataAccess.Repository.Contacts
{
	public interface ICustomerRepository : IRepository<Customer>
	{
		Customer GetCustomer(int id);
		Customer GetCustomerByUserId(string userId);
		Customer GetCustomerByToken(string token);
	}
}
