using Eshop.Api.DataAccess.Data;
using Eshop.Api.DataAccess.Repository.Contacts;
using Eshop.Api.Models.Contacts;
using Eshop.Api.Models.Orders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eshop.Api.DataAccess.Repository.Orders
{
	public class OrderProductRepository : Repository<OrderProduct>, IOrderProductRepository
	{
		public OrderProductRepository(ApplicationDbContext db) : base(db)
		{
		}

		public OrderProduct GetOrderProduct(int productId, int orderId)
		{
			return Get(o => o.ProductId == productId && o.OrderId == orderId);
		}
	}
}
