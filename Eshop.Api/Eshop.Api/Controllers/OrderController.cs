using Microsoft.AspNetCore.Mvc;

namespace Eshop.Api.Controllers
{
	public class OrderController : Controller
	{
		public IActionResult Index()
		{
			return View();
		}
	}
}
