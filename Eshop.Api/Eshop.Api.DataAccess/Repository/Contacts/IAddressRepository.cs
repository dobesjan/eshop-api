using Eshop.Api.Models.Contacts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eshop.Api.DataAccess.Repository.Contacts
{
	public interface IAddressRepository : IRepository<Address>
	{
		Address GetAddress(int id);
	}
}
