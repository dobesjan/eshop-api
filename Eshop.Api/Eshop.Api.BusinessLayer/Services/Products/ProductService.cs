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

			var product = _unitOfWork.ProductRepository.GetProduct(id);
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
				products = _unitOfWork.ProductRepository.GetProductsByCategory(categoryId, offset, limit);
			}
			else
			{
				products = _unitOfWork.ProductRepository.GetProducts(offset, limit);
			}

			if (products == null)
			{
				throw new ArgumentNullException($"Products for category {categoryId} with offset {offset} and limit {limit} are null!");
			}
			return products;
		}

		public int GetProductsCount(int categoryId = 0)
		{
			return _unitOfWork.ProductRepository.GetProductsCount(categoryId);
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

			var productImage = _unitOfWork.ProductImageRepository.Get(productId, imageId);
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
			var productImage = _unitOfWork.ProductImageRepository.Get(productId, imageId);
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
