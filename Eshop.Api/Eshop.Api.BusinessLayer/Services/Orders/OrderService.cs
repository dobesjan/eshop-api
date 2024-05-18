using Eshop.Api.BusinessLayer.Services.Interfaces.Orders;
using Eshop.Api.DataAccess.Repository.Interfaces;
using Eshop.Api.Models.Contacts;
using Eshop.Api.Models.Orders;
using Eshop.Api.Models.Products;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Eshop.Api.BusinessLayer.Services.Orders
{
	public class OrderService : EshopService, IOrderService
	{
		private readonly IRepository<Order> _ordersRepository;
		private readonly IRepository<OrderStatus> _ordersStatusRepository;
		private readonly IRepository<OrderProduct> _orderProductRepository;
		private readonly IRepository<OrderCustomer> _orderCustomerRepository;

		private readonly IRepository<OrderShipping> _orderShippingRepository;
		private readonly IRepository<Shipping> _shippingRepository;
		private readonly IRepository<ShippingPaymentMethod> _shippingPaymentMethodRepository;

		private readonly IRepository<Payment> _paymentRepository;
		private readonly IRepository<PaymentMethod> _paymentMethodRepository;
		private readonly IRepository<PaymentStatus> _paymentStatusRepository;

		private readonly IRepository<Address> _addressRepository;

		private readonly string _orderProperties = "OrderStatus,OrderProducts.Product,OrderShipping.Shipping,CustomerContacts.Customer,DeliveryAddress";

		private static void ValidateOrder(Order order)
		{
			if (order.UserId <= 0 || order.Token == String.Empty)
			{
				throw new InvalidDataException("UserId or token not provided!");
			}

			if (order.OrderProducts == null)
			{
				throw new InvalidDataException("There are not any products in order!");
			}

			if (order.OrderProducts.Any())
			{
				throw new InvalidDataException("There are not any products in order!");
			}
		}

		private void CheckOrderIsStored(int orderId)
		{
			if (!_ordersRepository.IsStored(orderId))
			{
				throw new InvalidDataException("Order not found in db!");
			}
		}

		public bool CreateOrder(Order order)
		{
			ValidateOrder(order);

			_ordersRepository.Add(order);
			return true;
		}

		public IEnumerable<Order> GetOrders(int offset = 0, int limit = 0)
		{
			if (limit > 0)
			{
				return _ordersRepository.GetAll(includeProperties: _orderProperties, offset: offset, limit: limit);
			}

			return _ordersRepository.GetAll(includeProperties: _orderProperties);
		}

		public Order GetOrder(int orderId = 0)
		{
			if (orderId <= 0)
			{
				throw new InvalidDataException("Order not found in db!");
			}

			var order = _ordersRepository.Get(c => c.Id == orderId, includeProperties: _orderProperties);
			if (order == null)
			{
				throw new InvalidDataException("Order not found in db!");
			}

			return order;
		}

		public IEnumerable<Order> GetOrdersForAnonymousUser(string token = "")
		{
			if (token == String.Empty)
			{
				throw new InvalidDataException("Wrong token!");
			}

			var orders = _ordersRepository.GetAll(c => c.Token == token, includeProperties: _orderProperties);
			if (orders == null)
			{
				throw new InvalidDataException("Order not found in db!");
			}

			return orders;
		}

		public IEnumerable<Order> GetOrdersForUser(int userId = 0)
		{
			if (userId <= 0)
			{
				throw new InvalidDataException("Wrong user!");
			}

			var orders = _ordersRepository.GetAll(c => c.UserId == userId, includeProperties: _orderProperties);
			if (orders == null)
			{
				throw new InvalidDataException("Order not found in db!");
			}

			return orders;
		}

		public bool LinkDeliveryAddressToOrder(int orderId, Address address)
		{
			var order = _ordersRepository.Get(pi => pi.Id == orderId);
			
			if (order == null)
			{
				throw new InvalidDataException("Order not found in db!");
			}

			if (UpsertEntity(address, _addressRepository))
			{
				var addressId = _addressRepository.Get(a => a.City == address.City && a.Street == address.Street && a.Country == address.Country)?.Id;

				if (!addressId.HasValue)
				{
					throw new InvalidDataException("Address not found in db!");
				}

				order.AddressId = addressId.Value;
				return true;
			}

			throw new InvalidDataException("Error!");
		}

		public bool SendOrder(Order order)
		{
			if (!order.IsValid())
			{
				throw new InvalidDataException("Order is not valid!");
			}

			if (!_ordersRepository.IsStored(order.Id))
			{
				throw new InvalidDataException("Order not found in db!");
			}

			order.SentTime = DateTime.UtcNow;
			order.IsOrdered = true;

			_ordersRepository.Update(order);
			return true;
		}

		public bool UpdateOrder(Order order)
		{
			ValidateOrder(order);

			return UpsertEntity(order, _ordersRepository);
		}

		public bool UpsertOrder(Order order)
		{
			CheckOrderIsStored(order.Id);
			throw new NotImplementedException();
		}

		public bool UpdateOrderStatus(OrderStatus status, Order order)
		{
			throw new NotImplementedException();
		}

		public bool AddProductToOrder(OrderProduct product, int orderId)
		{
			throw new NotImplementedException();
		}

		public bool AddProductsToOrder(IEnumerable<OrderProduct> products, int orderId)
		{
			throw new NotImplementedException();
		}

		public bool RemoveProductFromOrder(OrderProduct product, int orderId)
		{
			throw new NotImplementedException();
		}

		public bool RemoveProductsFromOrder(IEnumerable<OrderProduct> products, int orderId)
		{
			throw new NotImplementedException();
		}

		public bool UpdateProductCount(OrderProduct product, int orderId)
		{
			throw new NotImplementedException();
		}

		public bool UpdateProductsCount(IEnumerable<OrderProduct> products, int orderId)
		{
			throw new NotImplementedException();
		}

		public bool LinkCustomerContactToOrder(int orderId, int contactId)
		{
			if (!_orderCustomerRepository.IsStored(contactId))
			{
				throw new InvalidDataException("Customer not found in db!");
			}

			CheckOrderIsStored(orderId);

			var link = _orderCustomerRepository.Get(pi => pi.OrderId == orderId && pi.CustomerId == contactId);
			if (link == null)
			{
				var newLink = new OrderCustomer
				{
					OrderId = orderId,
					CustomerId = contactId
				};

				_orderCustomerRepository.Add(newLink);
				_orderCustomerRepository.Save();
			}

			return true;
		}

		public bool AddOrderPayment(Payment payment)
		{
			CheckOrderIsStored(payment.OrderId);
			_paymentRepository.Add(payment);
			_paymentRepository.Save();

			return true;
		}

		public bool UpdateOrderPayment(Payment payment)
		{
			CheckOrderIsStored(payment.OrderId);
			_paymentRepository.Update(payment);
			_paymentRepository.Save();

			return true;
		}

		public IEnumerable<Order> GetOrdersByShipping(int shippingId, int offset = 0, int limit = 0)
		{
			throw new NotImplementedException();
		}

		public IEnumerable<Order> GetOrdersByStatus(int orderStatusId, int offset = 0, int limit = 0)
		{
			throw new NotImplementedException();
		}

		public IEnumerable<Order> GetOrdersByDeliveryAddress(int orderStatusId, int offset = 0, int limit = 0)
		{
			throw new NotImplementedException();
		}

		public IEnumerable<Order> GetOrdersByCustomerContact(int orderStatusId, int offset = 0, int limit = 0)
		{
			throw new NotImplementedException();
		}
	}
}
