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

		private Expression<Func<Order, bool>> GetOrdersByStatusPredicate(int orderStatusId)
		{
			return o => o.OrderStatusId == orderStatusId;
		}

		public IEnumerable<Order> GetOrdersByStatus(int orderStatusId, int offset = 0, int limit = 0)
		{
			var predicate = GetOrdersByStatusPredicate(orderStatusId);
			return GetAll(predicate, includeProperties: _orderProperties, offset: offset, limit: limit);
		}

		public int GetOrdersCountByStatus(int orderStatusId)
		{
			var predicate = GetOrdersByStatusPredicate(orderStatusId);
			return Count(predicate);
		}

		private Expression<Func<Order, bool>> GetOrdersByShippingPredicate(int shippingId)
		{
			return o => o.ShippingId == shippingId;
		}

		public IEnumerable<Order> GetOrdersByShipping(int shippingId, int offset = 0, int limit = 0)
		{
			var predicate = GetOrdersByShippingPredicate(shippingId);
			return GetAll(predicate, includeProperties: _orderProperties, offset: offset, limit: limit);
		}

		public int GetOrdersCountByShipping(int shippingId)
		{
			var predicate = GetOrdersByShippingPredicate(shippingId);
			return Count(predicate);
		}

		public IEnumerable<Order> GetOrders(int offset = 0, int limit = 0)
		{
			return GetAll(includeProperties: _orderProperties, offset: offset, limit: limit);
		}

		public int GetOrdersCount()
		{
			return Count();
		}

		public IEnumerable<Order> GetOrders(Expression<Func<Order, bool>>? filter = null, int offset = 0, int limit = 0)
		{
			return GetAll(filter, includeProperties: _orderProperties, offset: offset, limit: limit);
		}

		public int GetOrdersCount(Expression<Func<Order, bool>>? filter = null)
		{
			return Count(filter);
		}

		private Expression<Func<Order, bool>> GetOrdersForAnonymousUserPredicate(string token)
		{
			return c => c.Token == token;
		}

		public IEnumerable<Order> GetOrdersForAnonymousUser(string token, int offset = 0, int limit = 0)
		{
			var predicate = GetOrdersForAnonymousUserPredicate(token);
			return GetAll(predicate, includeProperties: _orderProperties, offset, limit);
		}

		public int GetOrdersCountForAnonymousUser(string token)
		{
			var predicate = GetOrdersForAnonymousUserPredicate(token);
			return Count(predicate);
		}

		private Expression<Func<Order, bool>> GetOrdersForUserPredicate(int userId)
		{
			return c => c.UserId == userId;
		}

		public IEnumerable<Order> GetOrdersForUser(int userId, int offset = 0, int limit = 0)
		{
			var predicate = GetOrdersForUserPredicate(userId);
			return GetAll(predicate, includeProperties: _orderProperties, offset, limit);
		}

		public int GetOrdersCountForUser(int userId)
		{
			var predicate = GetOrdersForUserPredicate(userId);
			return Count(predicate);
		}
	}
}
