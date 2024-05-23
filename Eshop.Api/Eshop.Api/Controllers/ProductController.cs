using Eshop.Api.DataAccess.Repository;
using Eshop.Api.Models.Images;
using Eshop.Api.Models.Products;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json.Serialization;
using System.Text.Json;
using static System.Net.Mime.MediaTypeNames;
using static System.Runtime.InteropServices.JavaScript.JSType;
using Eshop.Api.BusinessLayer.Services.Products;

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
				return CreateResult(success: true, data: product.ToJson());
			}, "Product not found!");
		}

		[HttpGet("list")]
		public IActionResult ListProducts(int offset = 0, int limit = 0, int categoryId = 0)
		{
			return HandleResponse(() =>
			{
				var products = _productService.GetProducts(offset, limit, categoryId);
				var data = products.Select(c => c.ToJson()).ToList();
				return CreateResult(success: true, data: data);
			}, "Problem while retrieving products!");
		}

		[HttpGet("getProductsCount")]
		public IActionResult ListProducts(int categoryId = 0)
		{
			return HandleResponse(() =>
			{
				var count = _productService.GetProductsCount(categoryId);
				return CreateResult(success: true, data: new { count = count });
			}, "Problem while retrieving products!");
		}

		[HttpPost("upsert")]
		public IActionResult UpsertProduct([FromBody] Product product)
		{
			return HandleResponse(() =>
			{
				var success = _productService.UpsertProduct(product);
				return CreateResult(success: success, successMessage: "Product saved successfully!", errorMessage: "Problem while saving product!");
			}, "Problem while saving product!");
		}

		[HttpPost("upsertProductPrice")]
		public IActionResult UpsertProductPrice([FromBody] ProductPriceList priceList)
		{
			return HandleResponse(() =>
			{
				var success = _productService.UpsertProductPrice(priceList);
				return CreateResult(success: success, successMessage: "Product price saved successfully!", errorMessage: "Problem while saving product price!");
			}, "Problem while saving product price!");
		}

		[HttpGet("linkImage")]
		public IActionResult LinkImage(int productId, int imageId)
		{
			return HandleResponse(() =>
			{
				var success = _productService.LinkImageToProduct(productId, imageId);
				return CreateResult(success: success, successMessage: "Image linked successfully!", errorMessage: "Problem while linking image to product!");
			}, "Problem while linking image to product!");
		}

		[HttpGet("unlinkImage")]
		public IActionResult UnlinkImage(int productId, int imageId)
		{
			return HandleResponse(() =>
			{
				var success = _productService.UnlinkImageFromProduct(productId, imageId);
				return CreateResult(success: success, successMessage: "Image unlinked successfully!", errorMessage: "Problem while unlinking image from product!");
			}, "Problem while unlinking image from product!");
		}

		[HttpPost("addProductToCategory")]
		public IActionResult AddProductToCategory([FromBody] ProductCategory productCategory)
		{
			return HandleResponse(() =>
			{
				var success = _productService.AddProductToCategory(productCategory);
				return CreateResult(success: success, successMessage: "Product added to category successfully!", errorMessage: "Problem while adding product to category!");
			}, "Problem while adding product to category!");
		}
	}
}
