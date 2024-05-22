using Eshop.Api.BusinessLayer.Services.Orders;
using Eshop.Api.Models.Orders;
using Eshop.Api.Models.ViewModels.Contacts;
using Microsoft.AspNetCore.Mvc;

namespace Eshop.Api.Controllers
{
    [ApiController]
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

		[HttpGet("addProduct")]
		public IActionResult AddProduct(int productId, string token, int count)
		{
			return HandleResponse(() =>
			{
				var success = _orderService.AddProductToOrder(productId, token, count);
				return CreateResult(success: success, successMessage: "Product added succesfully!", errorMessage: "Problem while saving product to order!");
			}, "Problem while saving product to order!");
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

		[HttpGet("deleteProduct")]
		public IActionResult DeleteProduct(int productId, string token)
		{
			return HandleResponse(() =>
			{
				var success = _orderService.RemoveProductFromOrder(productId, token);
				return CreateResult(success: success, successMessage: "Product removed succesfully!", errorMessage: "Problem while removing product from order!");
			}, "Problem while removing product from order!");
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

		[HttpGet("updateStatus")]
		public IActionResult UpdateStatus(int statusId, string token)
		{
			return HandleResponse(() =>
			{
				var success = _orderService.UpdateOrderStatus(statusId, token);
				return CreateResult(success: success, successMessage: "Order status updated succesfully!", errorMessage: "Problem while updating Order status!");
			}, "Problem while updating Order status!");
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

		[HttpGet("sendOrder")]
		public IActionResult SendOrder(string token)
		{
			return HandleResponse(() =>
			{
				var success = _orderService.SendOrder(token);
				return CreateResult(success: success, successMessage: "Order sent!", errorMessage: "Problem while sending order!");
			}, "Problem while sending order!");
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

		[HttpGet("updateShipping")]
		public IActionResult UpdateShipping(int shippingId, string token)
		{
			return HandleResponse(() =>
			{
				var success = _orderService.UpdateShipping(shippingId, token);
				return CreateResult(success: success, successMessage: "Order shipping updated succesfully!", errorMessage: "Problem while updating Order shipping!");
			}, "Problem while updating Order shipping!");
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
		public IActionResult LinkDeliveryAddress([FromBody] AddressVM address)
		{
			return HandleResponse(() =>
			{
				var success = _orderService.LinkDeliveryAddressToOrder(address);
				return CreateResult(success: success, successMessage: "Address saved successfully!", errorMessage: "Problem while saving address!");
			}, "Problem while saving address!");
		}

		[HttpPost("linkCustomer")]
		public IActionResult LinkCustomer([FromBody] CustomerVM customerVM)
		{
			return HandleResponse(() =>
			{
				var success = _orderService.LinkCustomerContactToOrder(customerVM);
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

		[HttpGet("listForAnonymousUser")]
		public IActionResult ListOrdersForAnonymousUser(string token, int offset, int limit)
		{
			return HandleResponse(() =>
			{
				var orders = _orderService.GetOrdersForAnonymousUser(token, offset, limit);
				return CreateResult(success: true, data: orders.Select(o => o.ToJson()).ToList());
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

		[HttpGet("listByStatus")]
		public IActionResult ListOrdersByStatus(int orderStatusId, int offset, int limit)
		{
			return HandleResponse(() =>
			{
				var orders = _orderService.GetOrdersByStatus(orderStatusId, offset, limit);
				return CreateResult(success: true, data: orders.Select(o => o.ToJson()).ToList());
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
	}
}
