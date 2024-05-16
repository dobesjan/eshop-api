using Eshop.Api.DataAccess.Repository.Interfaces;
using Eshop.Api.Models.Images;
using Eshop.Api.Models.Products;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json.Serialization;
using System.Text.Json;
using Eshop.Api.BusinessLayer.Services.Interfaces.Products;
using static System.Net.Mime.MediaTypeNames;

namespace Eshop.Api.Controllers
{
	[ApiController]
	public class ProductController : EshopApiControllerBase
	{
		private IProductService _productService;
		private readonly ILogger _logger;

		public ProductController(IProductService productService, ILogger<ProductController> logger) 
		{
			_productService = productService;
			_logger = logger;
		}

		[HttpGet]
		[Route("api/[controller]/get")]
		public IActionResult GetProduct(int id = 0)
		{
			try
			{
				var product = _productService.GetProduct(id);
				return Json(new { data = product.ToJson() });
			}
			catch (Exception ex)
			{
				_logger.LogError(ex.Message);
				return Json(new { success = false, message = "Product not found!" });
			}
		}

		[HttpGet]
		[Route("api/[controller]/list")]
		public IActionResult ListProducts(int offset = 0, int limit = 0, int categoryId = 0)
		{
			try
			{
				var products = _productService.GetProducts(offset, limit, categoryId);
				var data = products.Select(c => c.ToJson()).ToList();
				return Json(new { products = data });
			}
			catch(Exception ex)
			{
				_logger.LogError(ex.Message);
				return Json(new { success = false, message = "Problem while retrieving products!" });
			}
		}

		[HttpPost]
		[Route("api/[controller]/upsert")]
		public IActionResult UpsertProduct([FromBody] Product product)
		{
			try
			{
				var success =  _productService.UpsertProduct(product);
				if (!success)
				{
					return Json(new { success = false, message = "Problem while saving product!" });
				}

				return Json(new { success = true, message = "Product saved succesfully!" });
			}
			catch(Exception ex)
			{
				_logger.LogError(ex.Message);
				return Json(new { success = false, message = "Problem while saving product!" });
			}
		}

		[HttpPost]
		[Route("api/[controller]/upsertProductPrice")]
		public IActionResult UpsertProductPrice([FromBody] ProductPriceList priceList)
		{
			try
			{
				var success = _productService.UpsertProductPrice(priceList);
				if (!success)
				{
					return Json(new { success = false, message = "Problem while saving product price!" });
				}

				return Json(new { success = true, message = "Product price saved succesfully!" });
			}
			catch (Exception ex)
			{
				_logger.LogError(ex.Message);
				return Json(new { success = false, message = "Problem while saving product price!" });
			}
		}

		[HttpGet]
		[Route("api/[controller]/linkImage")]
		public IActionResult LinkImage(int productId, int imageId)
		{
			try
			{
				var success = _productService.LinkImageToProduct(productId, imageId);
				if (!success)
				{
					return Json(new { success = false, message = "Problem while linking image to product!" });
				}

				return Json(new { success = true, message = "Image linked succesfully!" });
			}
			catch (Exception ex)
			{
				_logger.LogError(ex.Message);
				return Json(new { success = false, message = "Problem while linking image to product!" });
			}
		}

		[HttpGet]
		[Route("api/[controller]/unlinkImage")]
		public IActionResult UnlinkImage(int productId, int imageId)
		{
			try
			{
				var success = _productService.UnlinkImageFromProduct(productId, imageId);
				if (!success)
				{
					return Json(new { success = false, message = "Problem while unlinking image to product!" });
				}

				return Json(new { success = true, message = "Image unlinked succesfully!" });
			}
			catch (Exception ex)
			{
				_logger.LogError(ex.Message);
				return Json(new { success = false, message = "Problem while unlinking image to product!" });
			}
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

			try
			{
				var success = _productService.AddProductToCategory(productCategory);
				if (!success)
				{
					return Json(new { success = false, message = "Problem adding product to category!" });
				}

				return Json(new { success = true, message = "Product succesfully added to category!" });
			}
			catch (Exception ex)
			{
				_logger.LogError(ex.Message);
				return Json(new { success = false, message = "Problem adding product to category!" });
			}
		}
	}
}
