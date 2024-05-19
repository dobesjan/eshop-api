using Eshop.Api.DataAccess.Repository.Interfaces;
using Eshop.Api.Models;
using Eshop.Api.Models.Products;
using Microsoft.AspNetCore.Mvc;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Eshop.Api.Controllers
{
	public class EshopApiControllerBase : Controller
	{
		protected readonly ILogger _logger;

		public EshopApiControllerBase(ILogger logger)
		{
			_logger = logger;
		}

		protected IActionResult ValidateModel()
		{
			if (!ModelState.IsValid)
			{
				var errorMessages = ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage));
				return Json(new { success = false, message = "Validation failed", errors = errorMessages });
			}

			return null;
		}

		protected IActionResult HandleResponse(Func<IActionResult> action, string errorMessage)
		{
			try
			{
				return action();
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, errorMessage);
				return Json(new { success = false, message = errorMessage });
			}
		}

		protected IActionResult CreateResult(bool success, string successMessage, string errorMessage)
		{
			return Json(new { success, message = success ? successMessage : errorMessage });
		}

		protected IActionResult CreateResult(bool success, object data, string successMessage = "", string errorMessage = "")
		{
			return Json(new { success, message = success ? successMessage : errorMessage, data });
		}
	}
}
