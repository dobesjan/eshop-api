using Eshop.Api.DataAccess.Repository;
using Eshop.Api.Models;
using Eshop.Api.Models.Images;
using Eshop.Api.Models.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eshop.Api.BusinessLayer.Services.Products
{
    public class ProductService : EshopService, IProductService
	{
		private readonly IRepository<Category> _categoryRepository;
		private readonly IRepository<Product> _productRepository;
		private readonly IRepository<ProductCategory> _productCategoryRepository;
		private readonly IRepository<Image> _imageRepository;
		private readonly IRepository<ProductImage> _productImageRepository;
		private readonly IRepository<ProductPriceList> _productPriceListRepository;

		public ProductService(
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

		//TODO: Consider refactor (merge all of these types of methods to one).
		public bool AddProductToCategory(ProductCategory category)
		{
			if (!_productRepository.IsStored(category.ProductId))
			{
				throw new InvalidDataException("Product not found in db!");
			}

			if (!_categoryRepository.IsStored(category.CategoryId))
			{
				throw new InvalidDataException("Category not found in db!");
			}

			if (_productCategoryRepository.IsStored(category.Id))
			{
				return true;
			}

			return UpsertEntity(category, _productCategoryRepository) != null;
		}

		public Product GetProduct(int id)
		{
			if (id <= 0)
			{
				throw new InvalidDataException("Product not found in db!");
			}

			var product = _productRepository.Get(c => c.Id == id, includeProperties: "ProductCategories.Category,ProductImages.Image,ProductPrices,ProductPrices.Currency");
			if (product == null)
			{
				throw new InvalidDataException("Product not found in db!");
			}

			return product;
		}

		//TODO: Consider refactor (merge all of these types of methods to one).
		public IEnumerable<Product> GetProducts(int offset = 0, int limit = 0, int categoryId = 0)
		{
			IEnumerable<Product> products = null;
			var properties = "ProductCategories.Category,ProductImages.Image,ProductPrices,ProductPrices.Currency";

			if (categoryId > 0)
			{
				products = _productRepository.GetAll(p => p.ProductCategories != null && p.ProductCategories.Any(pc => pc.CategoryId == categoryId), includeProperties: properties, offset: offset, limit: limit);
			}
			else
			{
				products = _productRepository.GetAll(includeProperties: properties, offset: offset, limit: limit);
			}

			if (products == null)
			{
				throw new ArgumentNullException($"Products for category {categoryId} with offset {offset} and limit {limit} are null!");
			}
			return products;
		}

		//TODO: Consider refactor (merge all of these types of methods to one).
		public bool LinkImageToProduct(int productId, int imageId)
		{
			if (!_productRepository.IsStored(productId))
			{
				throw new InvalidDataException("Product not found in db!");
			}

			if (!_imageRepository.IsStored(imageId))
			{
				throw new InvalidDataException("Image not found in db!");
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
			}

			return true;
		}

		//TODO: Consider refactor (merge all of these types of methods to one).
		public bool UnlinkImageFromProduct(int productId, int imageId)
		{
			var productImage = _productImageRepository.Get(pi => pi.ProductId == productId && pi.ImageId == imageId);
			if (productImage != null)
			{
				_productImageRepository.Remove(productImage);
				_productImageRepository.Save();
			}
			
			return true;
		}

		public bool UpsertProduct(Product product)
		{
			return UpsertEntity(product, _productRepository) != null;
		}

		public bool UpsertProductPrice(ProductPriceList productPriceList)
		{
			return UpsertEntity(productPriceList, _productPriceListRepository) != null;
		}
	}
}
