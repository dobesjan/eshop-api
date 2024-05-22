using Eshop.Api.DataAccess.Repository;
using Eshop.Api.DataAccess.UnitOfWork;
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
		private readonly IUnitOfWork _unitOfWork;
		private readonly string _properties = "ProductCategories.Category,ProductImages.Image,ProductPrices,ProductPrices.Currency";

		public ProductService(IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;
		}

		//TODO: Consider refactor (merge all of these types of methods to one).
		public bool AddProductToCategory(ProductCategory category)
		{
			if (!_unitOfWork.ProductRepository.IsStored(category.ProductId))
			{
				throw new InvalidDataException("Product not found in db!");
			}

			if (!_unitOfWork.CategoryRepository.IsStored(category.CategoryId))
			{
				throw new InvalidDataException("Category not found in db!");
			}

			if (_unitOfWork.ProductCategoryRepository.IsStored(category.Id))
			{
				return true;
			}

			return UpsertEntity(category, _unitOfWork.ProductCategoryRepository) != null;
		}

		public Product GetProduct(int id)
		{
			if (id <= 0)
			{
				throw new InvalidDataException("Product not found in db!");
			}

			var product = _unitOfWork.ProductRepository.Get(c => c.Id == id, includeProperties: _properties);
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

			if (categoryId > 0)
			{
				products = _unitOfWork.ProductRepository.GetAll(p => p.ProductCategories != null && p.ProductCategories.Any(pc => pc.CategoryId == categoryId), includeProperties: _properties, offset: offset, limit: limit);
			}
			else
			{
				products = _unitOfWork.ProductRepository.GetAll(includeProperties: _properties, offset: offset, limit: limit);
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
			if (!_unitOfWork.ProductRepository.IsStored(productId))
			{
				throw new InvalidDataException("Product not found in db!");
			}

			if (!_unitOfWork.ImageRepository.IsStored(imageId))
			{
				throw new InvalidDataException("Image not found in db!");
			}

			var productImage = _unitOfWork.ProductImageRepository.Get(pi => pi.ProductId == productId && pi.ImageId == imageId);
			if (productImage == null)
			{
				var link = new ProductImage
				{
					ProductId = productId,
					ImageId = imageId
				};

				_unitOfWork.ProductImageRepository.Add(link);
				_unitOfWork.ProductImageRepository.Save();
			}

			return true;
		}

		//TODO: Consider refactor (merge all of these types of methods to one).
		public bool UnlinkImageFromProduct(int productId, int imageId)
		{
			var productImage = _unitOfWork.ProductImageRepository.Get(pi => pi.ProductId == productId && pi.ImageId == imageId);
			if (productImage != null)
			{
				_unitOfWork.ProductImageRepository.Remove(productImage);
				_unitOfWork.ProductImageRepository.Save();
			}
			
			return true;
		}

		public bool UpsertProduct(Product product)
		{
			return UpsertEntity(product, _unitOfWork.ProductRepository) != null;
		}

		public bool UpsertProductPrice(ProductPriceList productPriceList)
		{
			return UpsertEntity(productPriceList, _unitOfWork.ProductPriceListRepository) != null;
		}
	}
}
