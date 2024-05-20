using Eshop.Api.DataAccess.Data;
using Eshop.Api.Models.Contacts;
using Eshop.Api.Models.Orders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eshop.Api.DataAccess.Repository.Contacts
{
	public class AddressRepository : Repository<Address>, IAddressRepository
	{
		public AddressRepository(ApplicationDbContext db) : base(db)
		{
		}

		public Address GetAddress(int id)
		{
			return Get(a => a.Id == id);
		}
	}
}
