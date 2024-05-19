﻿using Eshop.Api.BusinessLayer.Services.Interfaces.Orders;
using Eshop.Api.DataAccess.Repository;
using Eshop.Api.DataAccess.Repository.Interfaces;
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
	public class OrderService : EshopService, IOrderService
	{
		private readonly IRepository<Order> _ordersRepository;
		private readonly IRepository<OrderStatus> _ordersStatusRepository;
		private readonly IRepository<OrderProduct> _orderProductRepository;

		private readonly IRepository<Shipping> _shippingRepository;
		private readonly IRepository<ShippingPaymentMethod> _shippingPaymentMethodRepository;

		private readonly IRepository<Payment> _paymentRepository;
		private readonly IRepository<PaymentMethod> _paymentMethodRepository;
		private readonly IRepository<PaymentStatus> _paymentStatusRepository;

		private readonly IRepository<Address> _addressRepository;
		private readonly IRepository<Person> _personRepository;
		private readonly IRepository<Customer> _customerRepository;

		private readonly IRepository<Product> _productRepository;

		private readonly string _orderProperties = "OrderStatus,OrderProducts.Product,Shipping,Customer,DeliveryAddress,Payment.PaymentStatus,Payment.PaymentMethod";

		public OrderService(IRepository<Order> ordersRepository, IRepository<OrderStatus> ordersStatusRepository, IRepository<OrderProduct> orderProductRepository, IRepository<Shipping> shippingRepository, IRepository<ShippingPaymentMethod> shippingPaymentMethodRepository, IRepository<Payment> paymentRepository, IRepository<PaymentMethod> paymentMethodRepository, IRepository<PaymentStatus> paymentStatusRepository, IRepository<Address> addressRepository, IRepository<Person> personRepository, IRepository<Customer> customerRepository, IRepository<Product> productRepository)
		{
			_ordersRepository = ordersRepository;
			_ordersStatusRepository = ordersStatusRepository;
			_orderProductRepository = orderProductRepository;
			_shippingRepository = shippingRepository;
			_shippingPaymentMethodRepository = shippingPaymentMethodRepository;
			_paymentRepository = paymentRepository;
			_paymentMethodRepository = paymentMethodRepository;
			_paymentStatusRepository = paymentStatusRepository;
			_addressRepository = addressRepository;
			_personRepository = personRepository;
			_customerRepository = customerRepository;
			_productRepository = productRepository;
		}

		#region Order
		private void CheckOrderIsStored(int orderId)
		{
			if (!_ordersRepository.IsStored(orderId))
			{
				throw new InvalidDataException("Order not found in db!");
			}
		}

		public bool CreateOrder(Order order)
		{
			if (order == null) throw new ArgumentNullException("Order is null");
			order.Validate();

			_ordersRepository.Add(order);
			_ordersRepository.Save();
			return true;
		}

		public IEnumerable<Order> GetOrders(int offset = 0, int limit = 0)
		{
			return _ordersRepository.GetAll(includeProperties: _orderProperties, offset: offset, limit: limit);
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
		public bool SendOrder(Order order)
		{
			if (order == null) throw new ArgumentNullException("Order is null");
			order.IsReadyToSend();

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
			if (order == null) throw new ArgumentNullException("Order is null");
			order.Validate();

			if (order.Id > 0)
			{
				var selectedOrder = GetOrder(order.Id);

				if (selectedOrder == null) throw new InvalidDataException($"Order with id: {order.Id} not found in db!");
				if (selectedOrder.IsOrdered) throw new InvalidDataException($"Order with id: {order.Id} was already sent!");

				_ordersRepository.Update(order);
				_ordersRepository.Save();

				return true;
			}

			throw new ArgumentNullException("Wrong identifier");
		}

		public bool UpsertOrder(Order order)
		{
			if (order == null) throw new ArgumentNullException("Order is null");
			order.Validate();

			//return UpsertEntity(order, _ordersRepository);

			if (order.Id > 0)
			{
				var selectedOrder = GetOrder(order.Id);

				if (selectedOrder == null) throw new InvalidDataException($"Order with id: {order.Id} not found in db!");
				if (selectedOrder.IsOrdered) throw new InvalidDataException($"Order with id: {order.Id} was already sent!");

				_ordersRepository.Update(order);
				_ordersRepository.Save();

				return true;
			}

			throw new ArgumentNullException("Wrong identifier");
		}

		public Order GetShoppingCart(int userId)
		{
			var cart = _ordersRepository.Get(o => o.UserId == userId && !o.IsOrdered);
			if (cart != null) return cart;

			cart = new Order
			{
				UserId = userId,
				OrderStatusId = 1,
				IsOrdered = false,
				CreatedDate = DateTime.UtcNow
			};

			return _ordersRepository.Add(cart, true);
		}

		public Order GetShoppingCart(string token)
		{
			var cart = _ordersRepository.Get(o => o.Token.Equals(token) && !o.IsOrdered);
			if (cart != null) return cart;

			cart = new Order
			{
				Token = token,
				OrderStatusId = 1,
				IsOrdered = false,
				CreatedDate = DateTime.UtcNow
			};

			return _ordersRepository.Add(cart, true);
		}

		public bool UpdateOrderStatus(OrderStatus status, Order order)
		{
			if (order == null) throw new ArgumentNullException("Order is null");
			CheckOrderIsStored(order.Id);

			if (!_ordersStatusRepository.IsStored(status.Id))
			{
				throw new InvalidDataException("Wrong order status");
			}

			order.OrderStatus = status;
			_ordersRepository.Add(order);
			_ordersRepository.Save();

			return true;
		}

		public IEnumerable<Order> GetOrdersByStatus(int orderStatusId, int offset = 0, int limit = 0)
		{
			return _ordersRepository.GetAll(o => o.OrderStatusId == orderStatusId, includeProperties: _orderProperties, offset: offset, limit: limit);
		}

		#endregion

		#region Contacts
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
				_ordersRepository.Update(order, true);

				return true;
			}

			throw new InvalidDataException("Error!");
		}

		public bool LinkCustomerContactToOrder(int orderId, Customer customer)
		{
			//TODO: Consider split - maybe to repository
			if (customer == null) throw new ArgumentNullException("Customer is null");
			if (customer.Person == null) throw new ArgumentNullException("Person is null");
			if (customer.Address == null) throw new ArgumentNullException("Address is null");

			customer.Person.Validate();
			customer.Address.Validate();

			var person = _personRepository.Add(customer.Person, true);
			var address = _addressRepository.Add(customer.Address, true);
			var newCustomer = new Customer
			{
				PersonId = person.Id,
				AddressId = address.Id,
			};

			_customerRepository.Add(newCustomer, true);

			return true;
		}

		#endregion

		#region Products
		private bool CreateProductToOrderRelation(int productId, int count, int orderId)
		{
			if (count <= 0) throw new InvalidDataException("Count must be higher than 0");
			var relation = _orderProductRepository.Get(o => o.ProductId == productId && o.OrderId == orderId);

			if (relation != null)
			{
				relation.ProductId = productId;
				relation.OrderId = orderId;
				relation.Count = count;

				_orderProductRepository.Update(relation, true);
			}
			else
			{
				var product = new OrderProduct
				{
					ProductId = productId,
					OrderId = orderId,
					Count = count
				};
				_orderProductRepository.Add(product, true);
			}

			return true;
		}

		public bool AddProductToOrder(OrderProduct product)
		{
			if (product == null) throw new ArgumentNullException("Order product is null");
			product.Validate();

			CheckOrderIsStored(product.OrderId);
			return CreateProductToOrderRelation(product.ProductId, product.Count, product.OrderId);
		}

		public bool AddProductToOrder(int productId, int userId, int count)
		{
			if (!_productRepository.IsStored(productId)) throw new InvalidDataException("Product not found");

			var cart = GetShoppingCart(userId);
			return CreateProductToOrderRelation(productId, count, cart.Id);
		}

		public bool AddProductToOrder(int productId, string token, int count)
		{
			if (!_productRepository.IsStored(productId)) throw new InvalidDataException("Product not found");

			var cart = GetShoppingCart(token);
			return CreateProductToOrderRelation(productId, count, cart.Id);
		}

		public bool AddProductsToOrder(IEnumerable<OrderProduct> products)
		{
			//TODO: Consider optimization - such as bulk inserts

			foreach (var product in products)
			{
				AddProductToOrder(product);
			}

			return true;
		}

		public bool RemoveProductFromOrder(int productId, int orderId)
		{
			var product = _orderProductRepository.Get(p => p.ProductId == productId && p.OrderId == orderId);
			if (product == null) throw new InvalidDataException("Product not found");

			_orderProductRepository.Remove(product);
			_orderProductRepository.Save();

			return true;
		}

		public bool UpdateProductCount(int productId, int count, int orderId)
		{
			var product = _orderProductRepository.Get(p => p.ProductId == productId && p.OrderId == orderId);
			if (product == null) throw new InvalidDataException("Product not found");

			product.Count = count;
			_orderProductRepository.Update(product, true);

			return true;
		}

		#endregion

		#region Payment
		private void CheckIfShippingSupportsPaymentMethod(int paymentId, int orderId)
		{
			var order = GetOrder(orderId);
			var shipping = _shippingRepository.Get(s => s.Id == order.ShippingId && s.Enabled == true && s.ShippingPaymentMethod != null && s.ShippingPaymentMethod.Exists(sp => sp.PaymentMethodId == paymentId), includeProperties: "ShippingPaymentMethod.PaymentMethod");
			if (shipping != null) throw new InvalidDataException("Payment not supported in provided shipping");
		}

		public bool AddOrderPayment(Payment payment)
		{
			CheckIfShippingSupportsPaymentMethod(payment.Id, payment.OrderId);

			_paymentRepository.Add(payment);
			_paymentRepository.Save();

			return true;
		}

		public bool UpdateOrderPayment(Payment payment)
		{
			CheckIfShippingSupportsPaymentMethod(payment.Id, payment.OrderId);

			CheckOrderIsStored(payment.OrderId);
			if (_paymentRepository.IsStored(payment.OrderId))
			_paymentRepository.Update(payment);
			_paymentRepository.Save();

			return true;
		}

		public IEnumerable<PaymentMethod> GetPaymentMethodsForShipping(int shippingId)
		{
			return _paymentMethodRepository.GetAll(pm => pm.ShippingPaymentMethod != null && pm.ShippingPaymentMethod.Exists(s => s.ShippingId == shippingId), includeProperties: "ShippingPaymentMethod");
		}

		#endregion

		#region Shipping

		public bool UpdateShipping(int shippingId, int orderId)
		{
			var shipping = _shippingRepository.Get(s => s.Id == shippingId && s.Enabled == true);
			if (shipping != null) throw new InvalidDataException("Shipping not supported");

			var order = GetOrder(orderId);
            order.ShippingId = shippingId;

			_ordersRepository.Update(order);
			_ordersRepository.Save();

            return true;
		}

		public IEnumerable<Order> GetOrdersByShipping(int shippingId, int offset = 0, int limit = 0)
		{
			IEnumerable<Order> orders = null;

			if (limit > 0)
			{
				orders = _ordersRepository.GetAll(includeProperties: _orderProperties, offset: offset, limit: limit);
			}
			else
			{
				orders = _ordersRepository.GetAll(includeProperties: _orderProperties);
			}

			if (shippingId > 0 && orders != null)
			{
				orders = orders.Where(o => o.Shipping.Id == shippingId);
			}

			if (orders == null)
			{
				throw new ArgumentNullException($"Products for shipping {shippingId} with offset {offset} and limit {limit} are null!");
			}
			return orders;
		}

		#endregion

		#region Filtering
		public IEnumerable<Order> GetOrdersByFilter(OrderFilter filter, int offset = 0, int limit = 0)
		{
			if (filter == null) throw new ArgumentNullException("Filter is null");

			/*
			return _ordersRepository.GetAll(
				o => o.Id == filter.OrderId && o.OrderStatus.Name.Contains(filter.OrderStatus) && o.IsOrdered == filter.IsOrdered
				&& o.CreatedDate >= filter.CreatedDateFrom && o.CreatedDate <= filter.CreatedDateTo && o.SentTime >= filter.SentTimeFrom
				&& o.SentTime <= filter.SentTimeTo && o.DeliveryTime >= filter.DeliveryTimeFrom && o.DeliveryTime <= filter.DeliveryTimeTo
				&& o.Shipping.Name.Contains(filter.Shipping) && o.Customer.Person.FirstName.Contains(filter.FirstName)
				&& o.Customer.Person.LastName.Contains(filter.LastName) && o.Customer.Person.Email.Contains(filter.Email)
				&& o.Customer.Person.PhoneNumber.Contains(filter.PhoneNumber) && o.Customer.Address.Street.Contains(filter.Street)
				&& o.Customer.Address.City.Contains(filter.City) && o.Customer.Address.Country.Contains(filter.Country)
				&& o.Customer.NewsletterAgree == filter.NewsletterAgree,
				includeProperties: _orderProperties,
				offset: offset,
				limit: limit);
			*/

			var parameter = Expression.Parameter(typeof(Order), "o");
			var expressions = new List<Expression>();

			if (filter.OrderId.HasValue)
				expressions.Add(Expression.Equal(Expression.Property(parameter, "Id"), Expression.Constant(filter.OrderId.Value)));

			if (!string.IsNullOrEmpty(filter.OrderStatus))
				expressions.Add(Expression.Call(Expression.Property(Expression.Property(parameter, "OrderStatus"), "Name"), "Contains", null, Expression.Constant(filter.OrderStatus)));

			if (filter.IsOrdered.HasValue)
				expressions.Add(Expression.Equal(Expression.Property(parameter, "IsOrdered"), Expression.Constant(filter.IsOrdered.Value)));

			if (filter.CreatedDateFrom.HasValue)
				expressions.Add(Expression.GreaterThanOrEqual(Expression.Property(parameter, "CreatedDate"), Expression.Constant(filter.CreatedDateFrom.Value)));

			if (filter.CreatedDateTo.HasValue)
				expressions.Add(Expression.LessThanOrEqual(Expression.Property(parameter, "CreatedDate"), Expression.Constant(filter.CreatedDateTo.Value)));

			if (filter.SentTimeFrom.HasValue)
				expressions.Add(Expression.GreaterThanOrEqual(Expression.Property(parameter, "SentTime"), Expression.Constant(filter.SentTimeFrom.Value)));

			if (filter.SentTimeTo.HasValue)
				expressions.Add(Expression.LessThanOrEqual(Expression.Property(parameter, "SentTime"), Expression.Constant(filter.SentTimeTo.Value)));

			if (filter.DeliveryTimeFrom.HasValue)
				expressions.Add(Expression.GreaterThanOrEqual(Expression.Property(parameter, "DeliveryTime"), Expression.Constant(filter.DeliveryTimeFrom.Value)));

			if (filter.DeliveryTimeTo.HasValue)
				expressions.Add(Expression.LessThanOrEqual(Expression.Property(parameter, "DeliveryTime"), Expression.Constant(filter.DeliveryTimeTo.Value)));

			if (!string.IsNullOrEmpty(filter.Shipping))
				expressions.Add(Expression.Call(Expression.Property(Expression.Property(parameter, "Shipping"), "Name"), "Contains", null, Expression.Constant(filter.Shipping)));

			if (!string.IsNullOrEmpty(filter.FirstName))
				expressions.Add(Expression.Call(Expression.Property(Expression.Property(Expression.Property(parameter, "Customer"), "Person"), "FirstName"), "Contains", null, Expression.Constant(filter.FirstName)));

			if (!string.IsNullOrEmpty(filter.LastName))
				expressions.Add(Expression.Call(Expression.Property(Expression.Property(Expression.Property(parameter, "Customer"), "Person"), "LastName"), "Contains", null, Expression.Constant(filter.LastName)));

			if (!string.IsNullOrEmpty(filter.Email))
				expressions.Add(Expression.Call(Expression.Property(Expression.Property(Expression.Property(parameter, "Customer"), "Person"), "Email"), "Contains", null, Expression.Constant(filter.Email)));

			if (!string.IsNullOrEmpty(filter.PhoneNumber))
				expressions.Add(Expression.Call(Expression.Property(Expression.Property(Expression.Property(parameter, "Customer"), "Person"), "PhoneNumber"), "Contains", null, Expression.Constant(filter.PhoneNumber)));

			if (!string.IsNullOrEmpty(filter.Street))
				expressions.Add(Expression.Call(Expression.Property(Expression.Property(Expression.Property(parameter, "Customer"), "Address"), "Street"), "Contains", null, Expression.Constant(filter.Street)));

			if (!string.IsNullOrEmpty(filter.City))
				expressions.Add(Expression.Call(Expression.Property(Expression.Property(Expression.Property(parameter, "Customer"), "Address"), "City"), "Contains", null, Expression.Constant(filter.City)));

			if (!string.IsNullOrEmpty(filter.PostalCode))
				expressions.Add(Expression.Call(Expression.Property(Expression.Property(Expression.Property(parameter, "Customer"), "Address"), "PostalCode"), "Contains", null, Expression.Constant(filter.PostalCode)));

			if (!string.IsNullOrEmpty(filter.Country))
				expressions.Add(Expression.Call(Expression.Property(Expression.Property(Expression.Property(parameter, "Customer"), "Address"), "Country"), "Contains", null, Expression.Constant(filter.Country)));

			if (filter.NewsletterAgree.HasValue)
				expressions.Add(Expression.Equal(Expression.Property(Expression.Property(parameter, "Customer"), "NewsletterAgree"), Expression.Constant(filter.NewsletterAgree.Value)));

			if (!expressions.Any())
			{
				throw new ArgumentException("No filter criteria provided.");
			}

			var body = expressions.Aggregate(Expression.AndAlso);
			var predicate = Expression.Lambda<Func<Order, bool>>(body, parameter);

			return _ordersRepository.GetAll(predicate, _orderProperties, offset, limit);
		}

		#endregion
	}
}
