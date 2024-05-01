using Eshop.Api.DataAccess.Repository.Interfaces;
using Eshop.Api.Models.Products;
using Microsoft.AspNetCore.Mvc;

namespace Eshop.Api.Controllers
{
	[ApiController]
	public class ProductController : Controller
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
			if (!ModelState.IsValid)
			{
				return Json(new { success = false, message = ModelState });
			}

			try
			{
				if (category != null)
				{
					if (category.Id > 0)
					{
						Category persistedCategory = _categoryRepository.Get(p => p.Id == category.Id);
						if (!_categoryRepository.IsStored(category.Id))
						{
							return Json(new { success = false, message = "Category not found in db!" });
						}

						_categoryRepository.Update(persistedCategory);
						_categoryRepository.Save();
						return Json(new { success = true, message = "Category update successfully" });
					}
					else
					{
						_categoryRepository.Add(category);
						_categoryRepository.Save();
						return Json(new { success = true, message = "Category created successfully" });
					}
				}
				else
				{
					return Json(new { success = false, message = "Category is null!" });
				}
			} catch (Exception ex)
			{
				return Json(new { success = false, message = ex });
			}
		}

		[HttpPost]
		[Route("api/[controller]/upsertProduct")]
		public IActionResult UpsertProduct([FromBody] Product product)
		{
			if (!ModelState.IsValid)
			{
				var errorMessages = ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage));
				return Json(new { success = false, message = "Validation failed", errors = errorMessages });
			}

			try
			{
				if (product != null)
				{
					if (product.Id > 0)
					{
						if (!_productRepository.IsStored(product.Id))
						{
							return Json(new { success = false, message = "Product not found in db!" });
						}

						_productRepository.Update(product);
						_productRepository.Save();
						return Json(new { success = true, message = "Product updated successfully" });
					}
					else
					{
						_productRepository.Add(product);
						_productRepository.Save();
						return Json(new { success = true, message = "Product created successfully" });
					}
				}
				else
				{
					return Json(new { success = false, message = "Product is null!" });
				}
				
			}
			catch (Exception ex)
			{
				return Json(new { success = false, message = ex.Message });
			}
		}

		[HttpPost]
		[Route("api/[controller]/addProductToCategory")]
		public IActionResult AddProductToCategory([FromBody] ProductCategory productCategory)
		{
			if (!ModelState.IsValid)
			{
				var errorMessages = ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage));
				return Json(new { success = false, message = "Validation failed", errors = errorMessages });
			}

			try
			{
				if (productCategory != null)
				{
					if (!_productRepository.IsStored(productCategory.ProductId))
					{
						return Json(new { success = false, message = "Product not found in db!" });
					}

					if (!_productRepository.IsStored(productCategory.CategoryId))
					{
						return Json(new { success = false, message = "Category not found in db!" });
					}

					if (_productCategoryRepository.IsStored(productCategory.Id))
					{
						return Json(new { success = false, message = "Category linked." });
					}

					_productCategoryRepository.Add(productCategory);
					_productCategoryRepository.Save();
					return Json(new { success = false, message = "Category linked." });
				}
				else
				{
					return Json(new { success = false, message = "Product is null!" });
				}

			}
			catch (Exception ex)
			{
				return Json(new { success = false, message = ex.Message });
			}
		}
	}
}
