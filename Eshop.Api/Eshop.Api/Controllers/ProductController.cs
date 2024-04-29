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

		public ProductController(IRepository<Category> categoryRepository, IRepository<Product> productRepository) 
		{
			_categoryRepository = categoryRepository;
			_productRepository = productRepository;
		}

		[HttpPost]
		[Route("api/[controller]/createCategory")]
		public IActionResult CreateCategory([FromBody] Category category)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}

			try
			{
				_categoryRepository.Add(category);
				_categoryRepository.Save();
			} catch (Exception ex)
			{
				return BadRequest(ex);
			}

			return Ok("Category created successfully");
		}

		[HttpPost]
		[Route("api/[controller]/createProduct")]
		public IActionResult CreateProduct([FromBody] Product product)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}

			try
			{
				_productRepository.Add(product);
				_productRepository.Save();
			}
			catch (Exception ex)
			{
				return BadRequest(ex);
			}

			return Ok("Product created successfully");
		}
	}
}
