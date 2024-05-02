using Eshop.Api.DataAccess.Repository.Interfaces;
using Eshop.Api.Models.Products;
using Microsoft.AspNetCore.Mvc;

namespace Eshop.Api.Controllers
{
	[ApiController]
	public class ProductController : EshopApiControllerBase
	{
		public IRepository<Category> _categoryRepository;
		public IRepository<Product> _productRepository;
		public IRepository<ProductCategory> _productCategoryRepository;

		public ProductController(IRepository<Category> categoryRepository, IRepository<Product> productRepository, IRepository<ProductCategory> productCategoryRepository) 
		{
			_categoryRepository = categoryRepository;
			_productRepository = productRepository;
			_productCategoryRepository = productCategoryRepository;
		}

		[HttpPost]
		[Route("api/[controller]/upsertCategory")]
		public IActionResult UpsertCategory([FromBody] Category category)
		{
			return UpsertEntity(category, _categoryRepository, true);
		}

		[HttpPost]
		[Route("api/[controller]/upsertProduct")]
		public IActionResult UpsertProduct([FromBody] Product product)
		{
			return UpsertEntity(product, _productRepository, true);
		}

		[HttpPost]
		[Route("api/[controller]/addProductToCategory")]
		public IActionResult AddProductToCategory([FromBody] ProductCategory productCategory)
		{
			var error = ValidateModel();
			if (error != null)
			{
				return error;
			}

			if (!_productRepository.IsStored(productCategory.ProductId))
			{
				return Json(new { success = false, message = "Product not found in db!" });
			}

			if (!_categoryRepository.IsStored(productCategory.CategoryId))
			{
				return Json(new { success = false, message = "Category not found in db!" });
			}

			if (_productCategoryRepository.IsStored(productCategory.Id))
			{
				return Json(new { success = false, message = "Category linked." });
			}


			return UpsertEntity(productCategory, _productCategoryRepository);
		}
	}
}
