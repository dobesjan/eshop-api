using Eshop.Api.Models.Orders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eshop.Api.BusinessLayer.Services.Orders
{
	public partial class OrderService : EshopService, IOrderService
	{
		public bool UpdateShipping(int shippingId, int customerId)
		{
			var cart = GetShoppingCart(customerId);

			var shipping = _unitOfWork.ShippingRepository.GetEnabledShipping(shippingId);
			if (shipping == null) throw new InvalidDataException("Shipping not supported");

			cart.ShippingId = shippingId;

			_unitOfWork.OrderRepository.Update(cart);
			_unitOfWork.OrderRepository.Save();

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
	}
}
