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

		[HttpPost("upsert")]
		public IActionResult UpsertOrder([FromBody] Order order)
		{
			return HandleResponse(() =>
			{
				var success = _orderService.UpsertOrder(order);
				return CreateResult(success: success, successMessage: "Order saved successfully!", errorMessage: "Problem while saving order!");
			}, "Problem while saving order!");
		}
	}
}
