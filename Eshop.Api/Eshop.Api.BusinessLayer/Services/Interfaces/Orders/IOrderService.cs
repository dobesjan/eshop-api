using Eshop.Api.Models.Contacts;
using Eshop.Api.Models.Orders;
using Eshop.Api.Models.Products;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Eshop.Api.BusinessLayer.Services.Interfaces.Orders
{
	public interface IOrderService : IEshopService
	{
		bool CreateOrder(Order order);
		bool UpdateOrder(Order order);
		bool UpsertOrder(Order order);
		bool UpdateOrderStatus(OrderStatus status, Order order);
		bool SendOrder(Order order);

		bool AddProductToOrder(OrderProduct product);
		bool AddProductsToOrder(IEnumerable<OrderProduct> products);
		bool RemoveProductFromOrder(int productId, int orderId);
		bool UpdateProductCount(int productId, int count, int orderId);

		bool LinkCustomerContactToOrder(int orderId, Customer customer);
		bool LinkDeliveryAddressToOrder(int orderId, Address address);

		bool AddOrderPayment(Payment payment);
		bool UpdateOrderPayment(Payment payment);
		IEnumerable<PaymentMethod> GetPaymentMethodsForShipping(int shippingId);

		Order GetOrder(int orderId = 0);
		IEnumerable<Order> GetOrders(int offset = 0, int limit = 0);
		IEnumerable<Order> GetOrdersByShipping(int shippingId, int offset = 0, int limit = 0);
		IEnumerable<Order> GetOrdersByStatus(int orderStatusId, int offset = 0, int limit = 0);
		IEnumerable<Order> GetOrdersByFilter(OrderFilter filter, int offset = 0, int limit = 0);
		IEnumerable<Order> GetOrdersForUser(int userId = 0);
		IEnumerable<Order> GetOrdersForAnonymousUser(string token = "");

		bool UpdateShipping(int shippingId, int orderId);
	}
}
