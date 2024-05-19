using Eshop.Api.BusinessLayer.Services.Interfaces.Orders;
using Eshop.Api.Models.Orders;
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
	}
}
