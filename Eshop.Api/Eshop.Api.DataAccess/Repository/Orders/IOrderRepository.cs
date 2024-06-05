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
		Order GetShoppingCart(int customerId);
		Order GetOrder(int orderId);
		IEnumerable<Order> GetOrders(int offset = 0, int limit = 0);
		int GetOrdersCount();
		IEnumerable<Order> GetOrders(Expression<Func<Order, bool>>? filter = null, int offset = 0, int limit = 0);
		int GetOrdersCount(Expression<Func<Order, bool>>? filter = null);
		IEnumerable<Order> GetOrdersByStatus(int orderStatusId, int offset = 0, int limit = 0);
		int GetOrdersCountByStatus(int orderStatusId);
		IEnumerable<Order> GetOrdersByShipping(int shippingId, int offset = 0, int limit = 0);
		int GetOrdersCountByShipping(int shippingId);
		IEnumerable<Order> GetOrdersForUser(int customerId, int offset = 0, int limit = 0);
		int GetOrdersCountForUser(int customerId);
	}
}
