using Eshop.Api.DataAccess.Repository.Interfaces;
using Eshop.Api.Models;
using Eshop.Api.Models.Products;
using Microsoft.AspNetCore.Mvc;

namespace Eshop.Api.Controllers
{
	public class EshopApiControllerBase : Controller
	{
		protected IActionResult UpsertEntity<T>(T entity, IRepository<T> repository, bool validate=false) where T: Entity
		{
			if (validate)
			{
				var error = ValidateModel();
				if (error != null)
				{
					return error;
				}
			}

			try
			{
				if (entity != null)
				{
					if (entity.Id > 0)
					{
						if (!repository.IsStored(entity.Id))
						{
							return Json(new { success = false, message = $"{nameof(entity)} not found in db!" });
						}

						repository.Update(entity);
						repository.Save();
						return Json(new { success = true, message = $"{nameof(entity)} updated successfully" });
					}
					else
					{
						repository.Add(entity);
						repository.Save();
						return Json(new { success = true, message = $"{nameof(entity)} created successfully" });
					}
				}
				else
				{
					return Json(new { success = false, message = $"{nameof(entity)} is null!" });
				}

			}
			catch (Exception ex)
			{
				return Json(new { success = false, message = ex.Message });
			}
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
	}
}
