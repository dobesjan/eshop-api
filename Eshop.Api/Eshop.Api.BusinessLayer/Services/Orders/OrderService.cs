using Eshop.Api.BusinessLayer.Services.Contacts;
using Eshop.Api.BusinessLayer.Services.Currencies;
using Eshop.Api.DataAccess.Repository;
using Eshop.Api.DataAccess.Repository.Contacts;
using Eshop.Api.DataAccess.Repository.Orders;
using Eshop.Api.DataAccess.UnitOfWork;
using Eshop.Api.Models;
using Eshop.Api.Models.Contacts;
using Eshop.Api.Models.Orders;
using Eshop.Api.Models.Products;
using Microsoft.EntityFrameworkCore.Metadata;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Eshop.Api.BusinessLayer.Services.Orders
{
    public partial class OrderService : EshopService, IOrderService
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly ICurrencyService _currencyService;
		private readonly ICustomerService _customerService;

		public OrderService(IUnitOfWork unitOfWork, ICurrencyService currencyService, ICustomerService customerService)
		{
			_unitOfWork = unitOfWork;
			_currencyService = currencyService;
			_customerService = customerService;
		}

		private void CheckOrderIsStored(int orderId)
		{
			if (!_unitOfWork.OrderRepository.IsStored(orderId))
			{
				throw new InvalidDataException("Order not found!");
			}
		}

		private bool UpdateOrderStatusInternal(int statusId, Order order)
		{
			if (!_unitOfWork.OrderStatusRepository.IsStored(statusId))
			{
				throw new InvalidDataException("Wrong order status");
			}

			order.OrderStatusId = statusId;
			_unitOfWork.OrderRepository.Update(order);
			_unitOfWork.OrderRepository.Save();

			return true;
		}

		private bool SendOrderInternal(Order order)
		{
			order.IsReadyToSend();

			order.SentTime = DateTime.UtcNow;
			order.IsOrdered = true;

			_unitOfWork.OrderRepository.Update(order);
			_unitOfWork.OrderRepository.Save();

			return true;
		}

		public IEnumerable<Order> GetOrders(int offset = 0, int limit = 0)
		{
			return _unitOfWork.OrderRepository.GetOrders(offset, limit);
		}

		public int GetOrdersCount()
		{
			return _unitOfWork.OrderRepository.GetOrdersCount();
		}

		public int GetOrdersCount(Expression<Func<Order, bool>>? filter = null)
		{
			return _unitOfWork.OrderRepository.GetOrdersCount(filter);
		}

		public Order GetOrder(int orderId = 0)
		{
			if (orderId <= 0)
			{
				throw new InvalidDataException("Order not found in db!");
			}

			var order = _unitOfWork.OrderRepository.GetOrder(orderId);
			if (order == null)
			{
				throw new InvalidDataException("Order not found in db!");
			}

			return order;
		}

		public IEnumerable<Order> GetOrdersForUser(int customerId = 0, int offset = 0, int limit = 0)
		{
			if (customerId <= 0)
			{
				throw new InvalidDataException("Wrong user!");
			}

			var orders = _unitOfWork.OrderRepository.GetOrdersForUser(customerId, offset, limit);
			if (orders == null)
			{
				throw new InvalidDataException("Order not found in db!");
			}

			return orders;
		}

		public int GetOrdersCountForUser(int customerId)
		{
			return _unitOfWork.OrderRepository.GetOrdersCountForUser(customerId);
		}

		public bool SendOrder(int customerId)
		{
			var order = GetShoppingCart(customerId);
			SendOrderInternal(order);

			return true;
		}

		public bool UpdateOrder(Order order)
		{
			if (order == null) throw new ArgumentNullException("Order is null");
			order.Validate();

			if (order.Id > 0)
			{
				var selectedOrder = GetOrder(order.Id);

				if (selectedOrder == null) throw new InvalidDataException($"Order with id: {order.Id} not found in db!");
				if (selectedOrder.IsOrdered) throw new InvalidDataException($"Order with id: {order.Id} was already sent!");

				_unitOfWork.OrderRepository.Update(order);
				_unitOfWork.OrderRepository.Save();

				return true;
			}

			throw new ArgumentNullException("Wrong identifier");
		}

		public Order GetShoppingCart(int customerId)
		{
			var customer = _customerService.GetCustomer(customerId);
			if (customer == null) throw new ArgumentNullException("Unknown customer identity!");

			var cart = _unitOfWork.OrderRepository.GetShoppingCart(customer.Id);
			if (cart != null) return cart;

			var currency = _currencyService.GetPreferedCurrency(customer.Id);

			cart = new Order
			{
				CustomerId = customerId,
				OrderStatusId = 1,
				IsOrdered = false,
				CreatedDate = DateTime.UtcNow,
				CurrencyId = currency.Id
			};

			return _unitOfWork.OrderRepository.Add(cart, true);
		}

		public bool UpdateOrderStatus(int statusId, int customerId)
		{
			var order = GetShoppingCart(customerId);
			UpdateOrderStatusInternal(statusId, order);

			return true;
		}

		public IEnumerable<Order> GetOrdersByStatus(int orderStatusId, int offset = 0, int limit = 0)
		{
			return _unitOfWork.OrderRepository.GetOrdersByStatus(orderStatusId, offset, limit);
		}

		public int GetOrdersCountByStatus(int orderStatusId)
		{
			return _unitOfWork.OrderRepository.GetOrdersCountByStatus(orderStatusId);
		}
    }
}
