using Eshop.Api.DataAccess.Repository.Interfaces;
using Eshop.Api.Models.Products;
using Microsoft.AspNetCore.Mvc;

namespace Eshop.Api.Controllers
{
	[ApiController]
	public class CategoryController : EshopApiControllerBase
	{
		public IRepository<Category> _categoryRepository;

		public CategoryController(IRepository<Category> categoryRepository)
		{
			_categoryRepository = categoryRepository;
		}

		[HttpGet]
		[Route("api/[controller]/list")]
		public IActionResult ListCategories()
		{
			var categories = _categoryRepository.GetAll();
			return Json(new { categories });
		}

		[HttpGet]
		[Route("api/[controller]/get")]
		public IActionResult GetCategory(int id = 0)
		{
			if (id == 0)
			{
				return Json(new { success = false, message = "Category not found in db!" });
			}

			var category = _categoryRepository.Get(c => c.Id == id, includeProperties: "ParentCategory");
			if (category == null)
			{
				return Json(new { success = false, message = "Category not found in db!" });
			}

			return Json(new { category });
		}

		[HttpPost]
		[Route("api/[controller]/upsert")]
		public IActionResult UpsertCategory([FromBody] Category category)
		{
			return UpsertEntity(category, _categoryRepository, true);
		}
	}
}
