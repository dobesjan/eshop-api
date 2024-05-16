using Eshop.Api.BusinessLayer.Services.Interfaces.Products;
using Eshop.Api.DataAccess.Repository.Interfaces;
using Eshop.Api.Models;
using Eshop.Api.Models.Interfaces;
using Eshop.Api.Models.Products;
using Microsoft.AspNetCore.Mvc;

namespace Eshop.Api.Controllers
{
	[ApiController]
	public class CategoryController : EshopApiControllerBase
	{
		private readonly ICategoryService _categoryService;
		private readonly ILogger<CategoryController> _logger;

		public CategoryController(ICategoryService categoryService, ILogger<CategoryController> logger)
		{
			_categoryService = categoryService;
			_logger = logger;
		}

		[HttpGet]
		[Route("api/[controller]/list")]
		public IActionResult ListCategories()
		{
			var categories = _categoryService.GetCategories();
			return Json(new { categories });
		}

		[HttpGet]
		[Route("api/[controller]/get")]
		public IActionResult GetCategory(int id = 0)
		{
			try
			{
				var category = _categoryService.GetCategory(id);
				return Json(new { category });
			}
			catch (InvalidDataException ex)
			{
				_logger.LogError(ex.Message);
				return Json(new { success = false, message = "Category not found!" });
			}
			catch (Exception ex)
			{
				_logger.LogError(ex.Message);
				return Json(new { success = false, message = "Unknown error!" });
			}
		}

		[HttpPost]
		[Route("api/[controller]/upsert")]
		public IActionResult UpsertCategory([FromBody] Category category)
		{
			try
			{
				bool status = _categoryService.UpsertCategory(category);
				return Json(new { success = true, message = "Category updated successfully" });
			}
			catch (Exception ex)
			{
				_logger.LogError(ex.Message);
				return Json(new { success = false, message = "Fail" });
			}
		}
	}
}
