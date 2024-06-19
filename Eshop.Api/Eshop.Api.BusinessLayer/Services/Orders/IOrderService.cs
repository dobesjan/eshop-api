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
        bool UpdateOrder(Order order);
        bool UpdateOrderStatus(int statusId, int customerId);
        bool SendOrder(int customerId);

        bool AddProductToOrder(OrderProduct product);
        bool AddProductToOrder(int productId, int customerId, int count);
        bool RemoveProductFromOrder(int productId, int customerId);

        bool LinkDeliveryAddressToOrder(Address address);
        bool LinkBillingContactToOrder(Contact contact, int customerId);

        bool UpsertOrderPayment(Payment payment, bool recalculate = false);
        bool GeneratePayment(int orderId, int paymentMethodId);
        //TODO: Add update method for payment status
        IEnumerable<PaymentMethod> GetPaymentMethodsForShipping(int shippingId);

        Order GetOrder(int orderId = 0);
        Order GetShoppingCart(int customerId);
        IEnumerable<Order> GetOrders(int offset = 0, int limit = 0);
		int GetOrdersCount();
		int GetOrdersCount(Expression<Func<Order, bool>>? filter = null);
		IEnumerable<Order> GetOrdersByShipping(int shippingId, int offset = 0, int limit = 0);
		int GetOrdersCountByShipping(int shippingId);
		IEnumerable<Order> GetOrdersByStatus(int orderStatusId, int offset = 0, int limit = 0);
		int GetOrdersCountByStatus(int orderStatusId);
		IEnumerable<Order> GetOrdersByFilter(OrderFilter filter, int offset = 0, int limit = 0);
        int GetOrdersByFilterCount(OrderFilter filter);
		IEnumerable<Order> GetOrdersForUser(int customerId = 0, int offset = 0, int limit = 0);
		int GetOrdersCountForUser(int customerId);

		bool UpdateShipping(int shippingId, int customerId);
    }
}
