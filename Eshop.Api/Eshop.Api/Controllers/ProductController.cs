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
	[Route("api/[controller]")]
	public class ProductController : EshopApiControllerBase
	{
		private readonly IProductService _productService;

		public ProductController(IProductService productService, ILogger<ProductController> logger) : base(logger)
		{
			_productService = productService;
		}

		[HttpGet("get")]
		public IActionResult GetProduct(int id = 0)
		{
			return HandleResponse(() =>
			{
				var product = _productService.GetProduct(id);
				return Json(new { data = product.ToJson() });
			}, "Product not found!");
		}

		[HttpGet("list")]
		public IActionResult ListProducts(int offset = 0, int limit = 0, int categoryId = 0)
		{
			return HandleResponse(() =>
			{
				var products = _productService.GetProducts(offset, limit, categoryId);
				var data = products.Select(c => c.ToJson()).ToList();
				return Json(new { products = data });
			}, "Problem while retrieving products!");
		}

		[HttpPost("upsert")]
		public IActionResult UpsertProduct([FromBody] Product product)
		{
			return HandleResponse(() =>
			{
				var success = _productService.UpsertProduct(product);
				return Json(new { success, message = success ? "Product saved successfully!" : "Problem while saving product!" });
			}, "Problem while saving product!");
		}

		[HttpPost("upsertProductPrice")]
		public IActionResult UpsertProductPrice([FromBody] ProductPriceList priceList)
		{
			return HandleResponse(() =>
			{
				var success = _productService.UpsertProductPrice(priceList);
				return Json(new { success, message = success ? "Product price saved successfully!" : "Problem while saving product price!" });
			}, "Problem while saving product price!");
		}

		[HttpGet("linkImage")]
		public IActionResult LinkImage(int productId, int imageId)
		{
			return HandleResponse(() =>
			{
				var success = _productService.LinkImageToProduct(productId, imageId);
				return Json(new { success, message = success ? "Image linked successfully!" : "Problem while linking image to product!" });
			}, "Problem while linking image to product!");
		}

		[HttpGet("unlinkImage")]
		public IActionResult UnlinkImage(int productId, int imageId)
		{
			return HandleResponse(() =>
			{
				var success = _productService.UnlinkImageFromProduct(productId, imageId);
				return Json(new { success, message = success ? "Image unlinked successfully!" : "Problem while unlinking image from product!" });
			}, "Problem while unlinking image from product!");
		}

		[HttpPost("addProductToCategory")]
		public IActionResult AddProductToCategory([FromBody] ProductCategory productCategory)
		{
			return HandleResponse(() =>
			{
				var success = _productService.AddProductToCategory(productCategory);
				return Json(new { success, message = success ? "Product added to category successfully!" : "Problem while adding product to category!" });
			}, "Problem while adding product to category!");
		}
	}
}
