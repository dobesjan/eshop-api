﻿using Eshop.Api.BusinessLayer.Services.Currencies;
using Eshop.Api.DataAccess.Repository;
using Eshop.Api.DataAccess.Repository.Contacts;
using Eshop.Api.DataAccess.Repository.Orders;
using Eshop.Api.DataAccess.UnitOfWork;
using Eshop.Api.Models;
using Eshop.Api.Models.Contacts;
using Eshop.Api.Models.Orders;
using Eshop.Api.Models.Products;
using Eshop.Api.Models.ViewModels.Contacts;
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
		private readonly IUnitOfWork _unitOfWork;
		private readonly ICurrencyService _currencyService;

		public OrderService(IUnitOfWork unitOfWork, ICurrencyService currencyService)
		{
			_unitOfWork = unitOfWork;
			_currencyService = currencyService;
		}

		#region Order
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
			return true;
		}

		public bool CreateOrder(Order order)
		{
			if (order == null) throw new ArgumentNullException("Order is null");
			order.Validate();

			_unitOfWork.OrderRepository.Add(order);
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

		public IEnumerable<Order> GetOrdersForAnonymousUser(string token = "", int offset = 0, int limit = 0)
		{
			if (token == String.Empty)
			{
				throw new InvalidDataException("Wrong token!");
			}

			var orders = _unitOfWork.OrderRepository.GetOrdersForAnonymousUser(token, offset, limit);
			if (orders == null)
			{
				throw new InvalidDataException("Order not found in db!");
			}

			return orders;
		}

		public int GetOrdersCountForAnonymousUser(string token)
		{
			return _unitOfWork.OrderRepository.GetOrdersCountForAnonymousUser(token);
		}

		public IEnumerable<Order> GetOrdersForUser(int userId = 0, int offset = 0, int limit = 0)
		{
			if (userId <= 0)
			{
				throw new InvalidDataException("Wrong user!");
			}

			var orders = _unitOfWork.OrderRepository.GetOrdersForUser(userId, offset, limit);
			if (orders == null)
			{
				throw new InvalidDataException("Order not found in db!");
			}

			return orders;
		}

		public int GetOrdersCountForUser(int userId)
		{
			return _unitOfWork.OrderRepository.GetOrdersCountForUser(userId);
		}

		public bool SendOrder(string token)
		{
			var order = GetShoppingCart(token);
			SendOrderInternal(order);

			return true;
		}

		public bool SendOrder(int userId)
		{
			var order = GetShoppingCart(userId);
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

		public Order GetShoppingCart(int userId)
		{
			var cart = _unitOfWork.OrderRepository.GetShoppingCart(userId);
			if (cart != null) return cart;

			var currency = _currencyService.GetPreferedCurrency(userId);

			cart = new Order
			{
				UserId = userId,
				OrderStatusId = 1,
				IsOrdered = false,
				CreatedDate = DateTime.UtcNow,
				CurrencyId = currency.Id
			};

			return _unitOfWork.OrderRepository.Add(cart, true);
		}

		public Order GetShoppingCart(string token)
		{
			var cart = _unitOfWork.OrderRepository.GetShoppingCart(token);
			if (cart != null) return cart;

			var currency = _currencyService.GetPreferedCurrency(token);

			cart = new Order
			{
				Token = token,
				OrderStatusId = 1,
				IsOrdered = false,
				CreatedDate = DateTime.UtcNow,
				CurrencyId = currency.Id
			};

			return _unitOfWork.OrderRepository.Add(cart, true);
		}

		public bool UpdateOrderStatus(int statusId, int userId)
		{
			var order = GetShoppingCart(userId);
			UpdateOrderStatusInternal(statusId, order);

			return true;
		}

		public bool UpdateOrderStatus(int statusId, string token)
		{
			var order = GetShoppingCart(token);
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

		#endregion

		#region Contacts
		public bool LinkDeliveryAddressToOrder(AddressVM address)
		{
			Order order = null;

			if (address.Address == null) throw new ArgumentNullException("Address not provided");

			if (address.Address.UserId > 0)
			{
				order = GetShoppingCart(address.Address.UserId);
			}
			else
			{
				order = GetShoppingCart(address.Token);
			}
			
			if (order == null)
			{
				throw new InvalidDataException("Order not found in db!");
			}

			Address selectedAddress = null;

			if (order.AddressId.HasValue && order.AddressId > 0)
			{
				selectedAddress = _unitOfWork.AddressRepository.GetAddress(order.AddressId.Value);
				if (selectedAddress != null)
				{
					address.Address.Id = selectedAddress.Id;
				}
			}

			selectedAddress = UpsertEntity(address.Address, _unitOfWork.AddressRepository);

			if (selectedAddress != null)
			{
				order.AddressId = selectedAddress.Id;
				_unitOfWork.OrderRepository.Update(order, true);

				return true;
			}

			throw new InvalidDataException("Error!");
		}

		public bool LinkCustomerContactToOrder(CustomerVM customerVM)
		{
			//TODO: Consider split - maybe to repository
			if (customerVM == null) throw new ArgumentNullException("Customer is null");
			if (customerVM.Customer == null) throw new ArgumentNullException("Customer is null");
			if (customerVM.Customer.Person == null) throw new ArgumentNullException("Person is null");
			if (customerVM.Customer.Address == null) throw new ArgumentNullException("Address is null");

			Order order = null;

			if (customerVM.UserId > 0)
			{
				order = GetShoppingCart(customerVM.UserId);
			}
			else
			{
				order = GetShoppingCart(customerVM.Token);
			}

			if (order == null)
			{
				throw new InvalidDataException("Order not found in db!");
			}

			if (order.CustomerId.HasValue && order.CustomerId > 0)
			{
				var selectedCustomer = _unitOfWork.CustomerRepository.GetCustomer(order.CustomerId.Value);
				if (selectedCustomer != null)
				{
					customerVM.Customer.Id = selectedCustomer.Id;
					customerVM.Customer.PersonId = selectedCustomer.PersonId;
					customerVM.Customer.Person.Id = selectedCustomer.PersonId;
					customerVM.Customer.AddressId = selectedCustomer.AddressId;
					if (selectedCustomer.AddressId.HasValue)
					{
						customerVM.Customer.Address.Id = selectedCustomer.AddressId.Value;
					}
				}
			}

			customerVM.Customer.Person.Validate();
			customerVM.Customer.Address.Validate();

			var person = UpsertEntity(customerVM.Customer.Person, _unitOfWork.PersonRepository);
			var address = UpsertEntity(customerVM.Customer.Address, _unitOfWork.AddressRepository);
			if (person == null) throw new ArgumentNullException("Error storing person");
			if (address == null) throw new ArgumentNullException("Error storing address");

			var customer = UpsertEntity(customerVM.Customer, _unitOfWork.CustomerRepository);
			order.CustomerId = customer.Id;
			_unitOfWork.OrderRepository.Update(order, true);

			return true;
		}

		#endregion

		#region Products
		private bool CreateProductToOrderRelation(int productId, int count, Order order)
		{
			if (count <= 0) throw new InvalidDataException("Count must be higher than 0");
			var relation = _unitOfWork.OrderProductRepository.GetOrderProduct(productId, order.Id);

			if (relation != null)
			{
				relation.ProductId = productId;
				relation.OrderId = order.Id;
				relation.CurrencyId = order.CurrencyId;
				relation.Count = count;

				_unitOfWork.OrderProductRepository.Update(relation, true);
			}
			else
			{
				var product = new OrderProduct
				{
					ProductId = productId,
					OrderId = order.Id,
					CurrencyId = order.CurrencyId,
					Count = count
				};
				_unitOfWork.OrderProductRepository.Add(product, true);
			}

			return true;
		}

		private bool RemoveProductFromOrderInternal(int productId, int orderId)
		{
			var product = _unitOfWork.OrderProductRepository.GetOrderProduct(productId, orderId);
			if (product == null) throw new InvalidDataException("Product not found");

			_unitOfWork.OrderProductRepository.Remove(product);
			_unitOfWork.OrderProductRepository.Save();

			return true;
		}

		public bool AddProductToOrder(OrderProduct product)
		{
			if (product == null) throw new ArgumentNullException("Order product is null");
			product.Validate();

			var selectedOrder = GetOrder(product.OrderId);
			if (selectedOrder == null) throw new InvalidDataException("Order not found");

			return CreateProductToOrderRelation(product.ProductId, product.Count, selectedOrder);
		}

		public bool AddProductToOrder(int productId, int userId, int count)
		{
			if (!_unitOfWork.ProductRepository.IsStored(productId)) throw new InvalidDataException("Product not found");

			var cart = GetShoppingCart(userId);
			return CreateProductToOrderRelation(productId, count, cart);
		}

		public bool AddProductToOrder(int productId, string token, int count)
		{
			if (!_unitOfWork.ProductRepository.IsStored(productId)) throw new InvalidDataException("Product not found");

			var cart = GetShoppingCart(token);
			return CreateProductToOrderRelation(productId, count, cart);
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

		public bool RemoveProductFromOrder(int productId, int userId)
		{
			var cart = GetShoppingCart(userId);
			RemoveProductFromOrderInternal(productId, cart.Id);

			return true;
		}

		public bool RemoveProductFromOrder(int productId, string token)
		{
			var cart = GetShoppingCart(token);
			RemoveProductFromOrderInternal(productId, cart.Id);

			return true;
		}

		#endregion

		#region Payment
		private void CheckIfShippingSupportsPaymentMethod(int paymentId, int orderId)
		{
			var order = GetOrder(orderId);
			if (!order.ShippingId.HasValue) throw new InvalidDataException("Shipping method not provided");
			var shipping = _unitOfWork.ShippingRepository.GetEnabledShipping(order.ShippingId.Value);
			if (shipping != null 
				&& shipping.ShippingPaymentMethod != null 
				&& shipping.ShippingPaymentMethod.Exists(sp => sp.PaymentMethodId == paymentId)) throw new InvalidDataException("Payment not supported in provided shipping");
		}

		public bool UpsertOrderPayment(Payment payment)
		{
			if (!_unitOfWork.PaymentMethodRepository.IsPaymentMethodEnabled(payment.PaymentMethodId)) throw new InvalidDataException("Payment method not supported");
			if (!_unitOfWork.PaymentStatusRepository.IsStored(payment.PaymentStatusId)) throw new InvalidDataException($"Wrong payment status with id {payment.PaymentStatusId}");

			CheckIfShippingSupportsPaymentMethod(payment.Id, payment.OrderId);
			CheckOrderIsStored(payment.OrderId);
			return UpsertEntity(payment, _unitOfWork.PaymentRepository) != null;
		}

		public IEnumerable<PaymentMethod> GetPaymentMethodsForShipping(int shippingId)
		{
			//TODO: Fix
			return _unitOfWork.PaymentMethodRepository.GetAll(pm => pm.ShippingPaymentMethod != null && pm.ShippingPaymentMethod.Exists(s => s.ShippingId == shippingId), includeProperties: "ShippingPaymentMethod");
		}

		#endregion

		#region Shipping

		public bool UpdateShippingInternal(int shippingId, Order order)
		{
			var shipping = _unitOfWork.ShippingRepository.GetEnabledShipping(shippingId);
			if (shipping != null) throw new InvalidDataException("Shipping not supported");

            order.ShippingId = shippingId;

			_unitOfWork.OrderRepository.Update(order);
			_unitOfWork.OrderRepository.Save();

            return true;
		}

		public bool UpdateShipping(int shippingId, int userId)
		{
			var cart = GetShoppingCart(userId);
			UpdateShippingInternal(shippingId, cart);

			return true;
		}

		public bool UpdateShipping(int shippingId, string token)
		{
			var cart = GetShoppingCart(token);
			UpdateShippingInternal(shippingId, cart);

			return true;
		}

		public IEnumerable<Order> GetOrdersByShipping(int shippingId, int offset = 0, int limit = 0)
		{
			IEnumerable<Order> orders = null;

			if (shippingId > 0)
			{
				orders = _unitOfWork.OrderRepository.GetOrdersByShipping(shippingId, offset: offset, limit: limit);
			}
			else
			{
				orders = _unitOfWork.OrderRepository.GetOrders(offset: offset, limit: limit);
			}

			if (orders == null)
			{
				throw new ArgumentNullException($"Products for shipping {shippingId} with offset {offset} and limit {limit} are null!");
			}
			return orders;
		}

		public int GetOrdersCountByShipping(int shippingId)
		{
			return _unitOfWork.OrderRepository.GetOrdersCountByShipping(shippingId);
		}

		#endregion

		#region Filtering
		private Expression<Func<Order, bool>> GetOrdersFilteringExpression(OrderFilter filter)
		{
			if (filter == null) throw new ArgumentNullException(nameof(filter));

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
				return null;
			}

			var body = expressions.Aggregate(Expression.AndAlso);
			return Expression.Lambda<Func<Order, bool>>(body, parameter);
		}

		public IEnumerable<Order> GetOrdersByFilter(OrderFilter filter, int offset = 0, int limit = 0)
		{
			var expression = GetOrdersFilteringExpression(filter);

			if (expression == null)
			{
				return _unitOfWork.OrderRepository.GetOrders(offset: offset, limit: limit);
			}

			return _unitOfWork.OrderRepository.GetOrders(expression, offset, limit);
		}

		public int GetOrdersByFilterCount(OrderFilter filter)
		{
			var expression = GetOrdersFilteringExpression(filter);

			if (expression == null)
			{
				return _unitOfWork.OrderRepository.Count();
			}

			return _unitOfWork.OrderRepository.Count(expression);
		}

		#endregion
	}
}
