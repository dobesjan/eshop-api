using Eshop.Api.BusinessLayer.Services.Tokens;
using Eshop.UI.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Eshop.UI.Controllers
{
	public class HomeController : EshopBaseController
	{
		private readonly ILogger<HomeController> _logger;

		public HomeController(ILogger<HomeController> logger)
		{
			_logger = logger;
		}

		public IActionResult Index()
		{
			var sessionToken = SessionToken;
			return View();
		}

		public IActionResult Privacy()
		{
			return View();
		}

		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Error()
		{
			return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}
	}
}
