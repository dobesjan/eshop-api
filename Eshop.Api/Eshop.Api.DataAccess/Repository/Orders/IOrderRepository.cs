using Eshop.Api.Models.Currencies;
using Eshop.Api.Models.Orders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Eshop.Api.DataAccess.Repository.Orders
{
	public interface IOrderRepository : IRepository<Order>
	{
		Order GetShoppingCart(int userId);
		Order GetShoppingCart(string token);
		Order GetOrder(int orderId);
		IEnumerable<Order> GetOrders(int offset = 0, int limit = 0);
		IEnumerable<Order> GetOrders(Expression<Func<Order, bool>>? filter = null, int offset = 0, int limit = 0);
		IEnumerable<Order> GetOrdersByStatus(int orderStatusId, int offset = 0, int limit = 0);
		IEnumerable<Order> GetOrdersByShipping(int shippingId, int offset = 0, int limit = 0);
		IEnumerable<Order> GetOrdersForAnonymousUser(string token);
		IEnumerable<Order> GetOrdersForUser(int userId);
	}
}
