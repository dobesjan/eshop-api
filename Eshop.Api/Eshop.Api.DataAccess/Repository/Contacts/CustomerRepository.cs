using Eshop.Api.DataAccess.Data;
using Eshop.Api.DataAccess.Repository.Orders;
using Eshop.Api.Models.Contacts;
using Eshop.Api.Models.Orders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eshop.Api.DataAccess.Repository.Contacts
{
	public class CustomerRepository : Repository<Customer>, ICustomerRepository
	{
		private readonly string _properties = "Person,Address";

		public CustomerRepository(ApplicationDbContext db) : base(db)
		{
		}

		public Customer GetCustomer(int id)
		{
			return Get(a => a.Id == id, includeProperties: "Person,Address");
		}
	}
}
