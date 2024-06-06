using Eshop.Api.BusinessLayer.Services.Orders;
using Eshop.Api.Models.Contacts;
using Eshop.Api.Models.Orders;
using Eshop.Api.Models.ViewModels.Contacts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Eshop.Api.Controllers
{
    [ApiController]
	[Authorize]
	[Route("api/[controller]")]
	public class OrderController : EshopApiControllerBase
	{
		private readonly IOrderService _orderService;

		public OrderController(IOrderService orderService, ILogger<OrderController> logger) : base(logger)
		{
			_orderService = orderService;
		}

		[HttpPost("update")]
		public IActionResult UpdateOrder([FromBody] Order order)
		{
			return HandleResponse(() =>
			{
				var success = _orderService.UpdateOrder(order);
				return CreateResult(success: success, successMessage: "Order saved successfully!", errorMessage: "Problem while saving order!");
			}, "Problem while saving order!");
		}

		[HttpGet("addProductForUser")]
		public IActionResult AddProductForUser(int productId, int userId, int count)
		{
			return HandleResponse(() =>
			{
				var success = _orderService.AddProductToOrder(productId, userId, count);
				return CreateResult(success: success, successMessage: "Product added succesfully!", errorMessage: "Problem while saving product to order!");
			}, "Problem while saving product to order!");
		}

		[HttpGet("deleteProductForUser")]
		public IActionResult DeleteProductForUser(int productId, int userId)
		{
			return HandleResponse(() =>
			{
				var success = _orderService.RemoveProductFromOrder(productId, userId);
				return CreateResult(success: success, successMessage: "Product removed succesfully!", errorMessage: "Problem while removing product from order!");
			}, "Problem while removing product from order!");
		}

		[HttpGet("updateStatusForUser")]
		public IActionResult UpdateStatusForUser(int statusId, int userId)
		{
			return HandleResponse(() =>
			{
				var success = _orderService.UpdateOrderStatus(statusId, userId);
				return CreateResult(success: success, successMessage: "Order status updated succesfully!", errorMessage: "Problem while updating Order status!");
			}, "Problem while updating Order status!");
		}

		[HttpGet("sendOrderForUser")]
		public IActionResult SendOrderForUser(int userId)
		{
			return HandleResponse(() =>
			{
				var success = _orderService.SendOrder(userId);
				return CreateResult(success: success, successMessage: "Order sent!", errorMessage: "Problem while sending order!");
			}, "Problem while sending order!");
		}

		[HttpGet("updateShippingForUser")]
		public IActionResult UpdateShippingForUser(int shippingId, int userId)
		{
			return HandleResponse(() =>
			{
				var success = _orderService.UpdateShipping(shippingId, userId);
				return CreateResult(success: success, successMessage: "Order shipping updated succesfully!", errorMessage: "Problem while updating Order shipping!");
			}, "Problem while updating Order shipping!");
		}

		[HttpPost("linkDeliveryAddress")]
		public IActionResult LinkDeliveryAddress([FromBody] Address address)
		{
			return HandleResponse(() =>
			{
				var success = _orderService.LinkDeliveryAddressToOrder(address);
				return CreateResult(success: success, successMessage: "Address saved successfully!", errorMessage: "Problem while saving address!");
			}, "Problem while saving address!");
		}

		[HttpPost("linkCustomer")]
		public IActionResult LinkCustomer([FromBody] Customer customer)
		{
			return HandleResponse(() =>
			{
				var success = _orderService.LinkCustomerContactToOrder(customer);
				return CreateResult(success: success, successMessage: "Customer saved successfully!", errorMessage: "Problem while saving customer!");
			}, "Problem while saving customer!");
		}

		[HttpPost("savePayment")]
		public IActionResult SavePayment([FromBody] Payment payment)
		{
			return HandleResponse(() =>
			{
				var success = _orderService.UpsertOrderPayment(payment);
				return CreateResult(success: success, successMessage: "Payment saved successfully!", errorMessage: "Problem while saving payment!");
			}, "Problem while saving payment!");
		}

		[HttpGet("get")]
		public IActionResult GetOrder(int id = 0)
		{
			return HandleResponse(() =>
			{
				var order = _orderService.GetOrder(id);
				return CreateResult(success: true, data: order.ToJson());
			}, "Order not found!");
		}

		[HttpPost("list")]
		public IActionResult ListOrders([FromBody] OrderFilter filter)
		{
			return HandleResponse(() =>
			{
				var orders = _orderService.GetOrdersByFilter(filter);
				return CreateResult(success: true, data: orders.Select(o => o.ToJson()).ToList());
			}, "Orders not found!");
		}

		[HttpPost("getOrdersCount")]
		public IActionResult GetOrdersCount([FromBody] OrderFilter filter)
		{
			return HandleResponse(() =>
			{
				var count = _orderService.GetOrdersByFilterCount(filter);
				return CreateResult(success: true, data: new { count = count});
			}, "Orders not found!");
		}

		[HttpGet("listForUser")]
		public IActionResult ListOrdersForUser(int userId, int offset, int limit)
		{
			return HandleResponse(() =>
			{
				var orders = _orderService.GetOrdersForUser(userId, offset, limit);
				return CreateResult(success: true, data: orders.Select(o => o.ToJson()).ToList());
			}, "Orders not found!");
		}

		[HttpGet("getOrdersCountForUser")]
		public IActionResult GetOrdersCountForUser(int userId)
		{
			return HandleResponse(() =>
			{
				var count = _orderService.GetOrdersCountForUser(userId);
				return CreateResult(success: true, data: new { count = count });
			}, "Orders not found!");
		}

		[HttpGet("listByStatus")]
		public IActionResult ListOrdersByStatus(int orderStatusId, int offset, int limit)
		{
			return HandleResponse(() =>
			{
				var orders = _orderService.GetOrdersByStatus(orderStatusId, offset, limit);
				return CreateResult(success: true, data: orders.Select(o => o.ToJson()).ToList());
			}, "Orders not found!");
		}

		[HttpGet("getOrdersCountByStatus")]
		public IActionResult GetOrdersCountByStatus(int orderStatusId)
		{
			return HandleResponse(() =>
			{
				var count = _orderService.GetOrdersCountByStatus(orderStatusId);
				return CreateResult(success: true, data: new { count = count });
			}, "Orders not found!");
		}

		[HttpGet("listByShipping")]
		public IActionResult ListOrdersByShipping(int shippingId, int offset, int limit)
		{
			return HandleResponse(() =>
			{
				var orders = _orderService.GetOrdersByShipping(shippingId, offset, limit);
				return CreateResult(success: true, data: orders.Select(o => o.ToJson()).ToList());
			}, "Orders not found!");
		}

		[HttpGet("getOrdersCountByShipping")]
		public IActionResult GetOrdersCountByShipping(int shippingId)
		{
			return HandleResponse(() =>
			{
				var count = _orderService.GetOrdersCountByShipping(shippingId);
				return CreateResult(success: true, data: new { count = count });
			}, "Orders not found!");
		}
	}
}
