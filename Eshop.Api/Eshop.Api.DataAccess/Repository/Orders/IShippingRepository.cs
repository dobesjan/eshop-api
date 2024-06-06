using Eshop.Api.Models.Orders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eshop.Api.DataAccess.Repository.Orders
{
	public interface IShippingRepository : IRepository<Shipping>
	{
		Shipping GetEnabledShipping(int id);
		IEnumerable<Shipping> GetEnabledShippingOptions(bool enabled = true);
	}
}
