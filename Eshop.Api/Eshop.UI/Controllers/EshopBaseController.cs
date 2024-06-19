using Eshop.Api.BusinessLayer.Services.Contacts;
using Eshop.Api.BusinessLayer.Services.Tokens;
using Eshop.Api.Models.Contacts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;

namespace Eshop.UI.Controllers
{
	public class EshopBaseController : Controller
	{
        private readonly ICustomerService _customerService;
		protected readonly ILogger _logger;

        private readonly string _tokenVariableName = "SessionToken";
		public string SessionToken
		{
			get
			{
				return HttpContext.Items[_tokenVariableName] as string;
			}
		}

		public EshopBaseController(ICustomerService customerService, ILogger logger)
		{
			_customerService = customerService;
			_logger = logger;
		}

		public Customer GetCustomer()
		{
			Customer customer = null;

			if (User != null && User.Identity != null)
			{
				if (User.Identity.IsAuthenticated)
				{
                    var customerId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
                    customer = _customerService.GetCustomerByUserId(customerId);
					if (customer == null)
					{
						var message = $"Authenticated customer with identifier: {customerId} not found in database even when user is authenticated";
                        _logger.LogWarning(message);
						throw new UnauthorizedAccessException(message);
						//Redirect(Url.Action("Logout", "Account"));
                    }
					
					return customer;
                }
			}

			//TODO: retrieve customer by token
			if (String.IsNullOrEmpty(SessionToken))
			{
				var message = "Token was not generated on client site";
				_logger.LogWarning(message);
				throw new UnauthorizedAccessException(message);
			}

			customer = _customerService.GetCustomerByToken(SessionToken);
			if (customer != null) return customer;

			customer = new Customer
			{
				NewsletterAgree = false,
				IsLogged = false,
				Token = SessionToken
			};

			return _customerService.CreateCustomer(customer);
		}

        protected IActionResult HandleResponse(Func<IActionResult> action, IActionResult errorResult)
        {
            try
            {
                return action();
            }
            catch (InvalidDataException ex)
            {
                _logger.LogInformation(ex, ex.Message);
                ModelState.Clear();
                ModelState.AddModelError(string.Empty, ex.Message);
                return errorResult;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                ModelState.Clear();
                ModelState.AddModelError(string.Empty, "An error occurred while processing your request.");
                return errorResult;
            }
        }
    }
}
