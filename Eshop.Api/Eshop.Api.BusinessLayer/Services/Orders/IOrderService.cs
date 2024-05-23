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

namespace Eshop.Api.BusinessLayer.Services.Orders
{
    public interface IOrderService : IEshopService
    {
        bool CreateOrder(Order order);
        bool UpdateOrder(Order order);
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

        bool LinkCustomerContactToOrder(CustomerVM customer);
        bool LinkDeliveryAddressToOrder(AddressVM address);

        bool UpsertOrderPayment(Payment payment);
        IEnumerable<PaymentMethod> GetPaymentMethodsForShipping(int shippingId);

        Order GetOrder(int orderId = 0);
        Order GetShoppingCart(int userId);
        Order GetShoppingCart(string token);
        IEnumerable<Order> GetOrders(int offset = 0, int limit = 0);
		int GetOrdersCount();
		int GetOrdersCount(Expression<Func<Order, bool>>? filter = null);
		IEnumerable<Order> GetOrdersByShipping(int shippingId, int offset = 0, int limit = 0);
		int GetOrdersCountByShipping(int shippingId);
		IEnumerable<Order> GetOrdersByStatus(int orderStatusId, int offset = 0, int limit = 0);
		int GetOrdersCountByStatus(int orderStatusId);
		IEnumerable<Order> GetOrdersByFilter(OrderFilter filter, int offset = 0, int limit = 0);
        int GetOrdersByFilterCount(OrderFilter filter);
		IEnumerable<Order> GetOrdersForUser(int userId = 0, int offset = 0, int limit = 0);
		int GetOrdersCountForUser(int userId);
		IEnumerable<Order> GetOrdersForAnonymousUser(string token = "", int offset = 0, int limit = 0);
		int GetOrdersCountForAnonymousUser(string token);

		bool UpdateShipping(int shippingId, int userId);
        bool UpdateShipping(int shippingId, string token);
    }
}
