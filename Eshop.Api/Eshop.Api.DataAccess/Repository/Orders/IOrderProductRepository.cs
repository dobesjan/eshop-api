using Eshop.Api.Models.Contacts;
using Eshop.Api.Models.Orders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eshop.Api.DataAccess.Repository.Orders
{
	public interface IOrderProductRepository : IRepository<OrderProduct>
	{
		OrderProduct GetOrderProduct(int productId, int orderId);
	}
}
