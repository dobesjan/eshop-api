using Eshop.Api.BusinessLayer.Services.Interfaces.Products;
using Eshop.Api.Models;
using Eshop.Api.Models.Products;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Eshop.Api.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class CategoryController : EshopApiControllerBase
	{
		private readonly ICategoryService _categoryService;

		public CategoryController(ICategoryService categoryService, ILogger<CategoryController> logger) : base(logger)
		{
			_categoryService = categoryService;
		}

		[HttpGet("list")]
		public IActionResult ListCategories()
		{
			return HandleResponse(() =>
			{
				var categories = _categoryService.GetCategories();
				return Json(new { categories });
			}, "Problem while retrieving categories!");
		}

		[HttpGet("get")]
		public IActionResult GetCategory(int id = 0)
		{
			return HandleResponse(() =>
			{
				var category = _categoryService.GetCategory(id);
				return Json(new { category });
			}, "Category not found!");
		}

		[HttpPost("upsert")]
		public IActionResult UpsertCategory([FromBody] Category category)
		{
			return HandleResponse(() =>
			{
				bool success = _categoryService.UpsertCategory(category);
				return Json(new { success, message = success ? "Category updated successfully" : "Problem saving category!" });
			}, "Problem saving category!");
		}
	}
}
