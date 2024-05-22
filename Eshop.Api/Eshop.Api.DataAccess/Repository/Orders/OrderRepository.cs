using Eshop.Api.DataAccess.Data;
using Eshop.Api.Models.Orders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Eshop.Api.DataAccess.Repository.Orders
{
	public class OrderRepository : Repository<Order>, IOrderRepository
	{
		private readonly string _orderProperties = "OrderStatus,OrderProducts.Product,OrderProducts.Product.ProductPrices,Shipping,Customer,DeliveryAddress,Payment.PaymentStatus,Payment.PaymentMethod,Payment.Currency,Currency";

		public OrderRepository(ApplicationDbContext db) : base(db)
		{
		}

		public Order GetShoppingCart(int userId)
		{
			return Get(o => o.UserId == userId && !o.IsOrdered);
		}

		public Order GetShoppingCart(string token)
		{
			return Get(o => o.Token.Equals(token) && !o.IsOrdered);
		}

		public Order GetOrder(int orderId)
		{
			return Get(orderId, includeProperties: _orderProperties);
		}

		public IEnumerable<Order> GetOrdersByStatus(int orderStatusId, int offset = 0, int limit = 0)
		{
			return GetAll(o => o.OrderStatusId == orderStatusId, includeProperties: _orderProperties, offset: offset, limit: limit);
		}

		public IEnumerable<Order> GetOrdersByShipping(int shippingId, int offset = 0, int limit = 0)
		{
			return GetAll(o => o.ShippingId == shippingId, includeProperties: _orderProperties, offset: offset, limit: limit);
		}

		public IEnumerable<Order> GetOrders(int offset = 0, int limit = 0)
		{
			return GetAll(includeProperties: _orderProperties, offset: offset, limit: limit);
		}

		public IEnumerable<Order> GetOrders(Expression<Func<Order, bool>>? filter = null, int offset = 0, int limit = 0)
		{
			return GetAll(filter, includeProperties: _orderProperties, offset: offset, limit: limit);
		}

		public IEnumerable<Order> GetOrdersForAnonymousUser(string token, int offset = 0, int limit = 0)
		{
			return GetAll(c => c.Token == token, includeProperties: _orderProperties, offset, limit);
		}

		public IEnumerable<Order> GetOrdersForUser(int userId, int offset = 0, int limit = 0)
		{
			return GetAll(c => c.UserId == userId, includeProperties: _orderProperties, offset, limit);
		}
	}
}
