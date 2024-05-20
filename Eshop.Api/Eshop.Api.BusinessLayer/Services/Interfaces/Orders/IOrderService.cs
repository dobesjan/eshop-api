using Eshop.Api.Models.Contacts;
using Eshop.Api.Models.Orders;
using Eshop.Api.Models.Products;
using Eshop.Api.Models.ViewModels.Contacts;
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
		bool UpdateOrderStatus(int statusId, int userId);
		bool UpdateOrderStatus(int statusId, string token);
		bool SendOrder(string token);
		bool SendOrder(int userId);

		bool AddProductToOrder(OrderProduct product);
		bool AddProductToOrder(int productId, int userId, int count);
		bool AddProductToOrder(int productId, string token, int count);
		bool AddProductsToOrder(IEnumerable<OrderProduct> products);
		bool RemoveProductFromOrder(int productId, int userId);
		bool RemoveProductFromOrder(int productId, string token);
		bool UpdateProductCount(int productId, int count, int orderId);

		bool LinkCustomerContactToOrder(CustomerVM customer);
		bool LinkDeliveryAddressToOrder(AddressVM address);

		bool UpsertOrderPayment(Payment payment);
		IEnumerable<PaymentMethod> GetPaymentMethodsForShipping(int shippingId);

		Order GetOrder(int orderId = 0);
		Order GetShoppingCart(int userId);
		Order GetShoppingCart(string token);
		IEnumerable<Order> GetOrders(int offset = 0, int limit = 0);
		IEnumerable<Order> GetOrdersByShipping(int shippingId, int offset = 0, int limit = 0);
		IEnumerable<Order> GetOrdersByStatus(int orderStatusId, int offset = 0, int limit = 0);
		IEnumerable<Order> GetOrdersByFilter(OrderFilter filter, int offset = 0, int limit = 0);
		IEnumerable<Order> GetOrdersForUser(int userId = 0);
		IEnumerable<Order> GetOrdersForAnonymousUser(string token = "");

		bool UpdateShipping(int shippingId, int userId);
		bool UpdateShipping(int shippingId, string token);
	}
}
