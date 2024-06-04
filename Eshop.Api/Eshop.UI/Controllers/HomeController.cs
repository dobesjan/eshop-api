using Eshop.Api.BusinessLayer.Services.Contacts;
using Eshop.Api.BusinessLayer.Services.Tokens;
using Eshop.Api.DataAccess.Repository.Contacts;
using Eshop.Api.Models.Contacts;
using Eshop.UI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Net.Mail;
using System.Security.Claims;

namespace Eshop.UI.Controllers
{
	public class HomeController : EshopBaseController
	{
		private readonly ILogger<HomeController> _logger;
		private readonly ICustomerService _customerService;

		public HomeController(ILogger<HomeController> logger, ICustomerService customerService)
		{
			_logger = logger;
			_customerService = customerService;
		}

		public IActionResult Index()
		{
			var sessionToken = SessionToken;
			return View();
		}

		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Error()
		{
			return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}

        [Authorize]
        public IActionResult Callback(string returnUrl = "/")
        {
            var customerId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            var customer = _customerService.GetCustomerByUserId(customerId);
			if (customer == null)
			{
				var person = new Person
				{
					Email = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value,
                    FirstName = User.Claims.FirstOrDefault(c => c.Type.Contains("givenname"))?.Value,
					LastName = User.Claims.FirstOrDefault(c => c.Type.Contains("surname"))?.Value
				};

				customer = new Customer
				{
					NewsletterAgree = false,
					IsLogged = true,
					UserId = customerId,
					Person = person
				};

				try
				{
					_customerService.CreateCustomer(customer);
				}
				catch(InvalidDataException ex)
				{
					_logger.LogWarning(ex.Message);
					//TODO: Consider if we should be redirected to some error page
                }
			}

            return LocalRedirect(returnUrl);
        }
    }
}
