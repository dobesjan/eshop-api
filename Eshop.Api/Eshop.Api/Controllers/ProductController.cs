﻿using Eshop.Api.DataAccess.Repository.Interfaces;
using Eshop.Api.Models.Images;
using Eshop.Api.Models.Products;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json.Serialization;
using System.Text.Json;

namespace Eshop.Api.Controllers
{
	[ApiController]
	public class ProductController : EshopApiControllerBase
	{
		public IRepository<Category> _categoryRepository;
		public IRepository<Product> _productRepository;
		public IRepository<ProductCategory> _productCategoryRepository;
		public IRepository<Image> _imageRepository;
		public IRepository<ProductImage> _productImageRepository;
		public IRepository<ProductPriceList> _productPriceListRepository;

		public ProductController(
			IRepository<Category> categoryRepository,
			IRepository<Product> productRepository,
			IRepository<ProductCategory> productCategoryRepository,
			IRepository<Image> imageRepository,
			IRepository<ProductImage> productImageRepository,
			IRepository<ProductPriceList> productPriceListRepository
			) 
		{
			_categoryRepository = categoryRepository;
			_productRepository = productRepository;
			_productCategoryRepository = productCategoryRepository;
			_imageRepository = imageRepository;
			_productImageRepository = productImageRepository;
			_productPriceListRepository = productPriceListRepository;
		}

		[HttpGet]
		[Route("api/[controller]/listCategories")]
		public IActionResult ListCategories()
		{
			var categories = _categoryRepository.GetAll();
			return Json(new { categories });
		}

		[HttpGet]
		[Route("api/[controller]/getCategory")]
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

		[HttpGet]
		[Route("api/[controller]/getProduct")]
		public IActionResult GetProduct(int id = 0)
		{
			if (id == 0)
			{
				return Json(new { success = false, message = "Product not found in db!" });
			}

			var product = _productRepository.Get(c => c.Id == id, includeProperties: "ProductCategories.Category,ProductImages.Image,ProductPrices,ProductPrices.Currency");
			if (product == null)
			{
				return Json(new { success = false, message = "Product not found in db!" });
			}

			var data = new
			{
				Id = product.Id,
				Name = product.Name,
				Enabled = product.Enabled,
				IsInStock = product.IsInStock,
				BuyLimit = product.BuyLimit,
				Categories = product.ProductCategories != null && product.ProductCategories.Any() ? product.ProductCategories.Select(pc => pc.Category).ToList().Select(cc => new { Id = cc.Id, Name = cc.Name, Enabled = cc.Enabled }) : null,
				Images = product.ProductImages != null && product.ProductImages.Any() ? product.ProductImages.Select(pi => pi.Image).ToList().Select(i => new { Id = i.Id, FileName = i.FileName }) : null,
				Prices = product.ProductPrices != null && product.ProductPrices.Any() ? product.ProductPrices.ToList().Select(pr => new { Id = pr.Id, Cost = pr.Cost, CostWithTax = pr.CostWithTax, CostBefore = pr.CostBefore, Currency = pr.Currency }) : null
			};

			return Json(new { data });
		}

		[HttpGet]
		[Route("api/[controller]/listProducts")]
		public IActionResult ListProducts(int offset = 0, int limit = 0)
		{
			IEnumerable<Product> products = null;
			var properties = "ProductCategories.Category,ProductImages.Image,ProductPrices,ProductPrices.Currency";

			if (limit > 0)
			{
				products = _productRepository.GetAll(includeProperties: properties, offset: offset, limit: limit);
			}
			else
			{
				products = _productRepository.GetAll(includeProperties: properties);
			}

			var data = products.Select(c => new
			{
				Id = c.Id,
				Name = c.Name,
				Enabled = c.Enabled,
				IsInStock = c.IsInStock,
				BuyLimit = c.BuyLimit,
				Categories = c.ProductCategories != null && c.ProductCategories.Any() ? c.ProductCategories.Select(pc => pc.Category).ToList().Select(cc => new { Id = cc.Id, Name = cc.Name, Enabled = cc.Enabled }) : null,
				Images = c.ProductImages != null && c.ProductImages.Any() ? c.ProductImages.Select(pi => pi.Image).ToList().Select(i => new { Id = i.Id, FileName = i.FileName }) : null,
				Prices = c.ProductPrices != null && c.ProductPrices.Any() ? c.ProductPrices.ToList().Select(pr => new { Id = pr.Id, Cost = pr.Cost, CostWithTax = pr.CostWithTax, CostBefore = pr.CostBefore, Currency = pr.Currency }) : null
			}).ToList();

			return Json(new { products = data });
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
		[Route("api/[controller]/upsertProductPrice")]
		public IActionResult UpsertProductPrice([FromBody] ProductPriceList priceList)
		{
			return UpsertEntity(priceList, _productPriceListRepository, true);
		}

		[HttpGet]
		[Route("api/[controller]/linkImage")]
		public IActionResult LinkImage(int productId, int imageId)
		{
			if (!_productRepository.IsStored(productId))
			{
				return Json(new { success = false, message = "Product not found in db!" });
			}

			if (!_imageRepository.IsStored(imageId))
			{
				return Json(new { success = false, message = "Image not found in db!" });
			}

			var productImage = _productImageRepository.Get(pi => pi.ProductId == productId && pi.ImageId == imageId);
			if (productImage == null)
			{
				var link = new ProductImage
				{
					ProductId = productId,
					ImageId = imageId
				};

				_productImageRepository.Add(link);
				_productImageRepository.Save();

				return Json(new { success = true, message = "Image succesfully linked to product." });
			}
			else
			{
				return Json(new { success = true, message = "Image already linked." });
			}
		}

		[HttpGet]
		[Route("api/[controller]/unlinkImage")]
		public IActionResult UnlinkImage(int productId, int imageId)
		{
			var productImage = _productImageRepository.Get(pi => pi.ProductId == productId && pi.ImageId == imageId);
			if (productImage == null)
			{
				return Json(new { success = true, message = "Image already unlinked." });
			}
			else
			{
				_productImageRepository.Remove(productImage);
				_productImageRepository.Save();
				return Json(new { success = true, message = "Image unlinked." });
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
