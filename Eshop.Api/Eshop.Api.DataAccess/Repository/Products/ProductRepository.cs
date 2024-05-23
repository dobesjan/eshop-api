using Eshop.Api.DataAccess.Data;
using Eshop.Api.Models.Orders;
using Eshop.Api.Models.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Eshop.Api.DataAccess.Repository.Products
{
	public class ProductRepository : Repository<Product>, IProductRepository
	{
		private readonly string _properties = "ProductCategories.Category,ProductImages.Image,ProductPrices,ProductPrices.Currency";

		public ProductRepository(ApplicationDbContext db) : base(db)
		{
		}

		public Product GetProduct(int productId)
		{
			return Get(productId, _properties);
		}

		public IEnumerable<Product> GetProducts(int offset = 0, int limit = 0)
		{
			return GetAll(includeProperties: _properties, offset: offset, limit: limit);
		}

		public int GetProductsCount()
		{
			return Count();
		}

		private Expression<Func<Product, bool>> GetProductsByCategoryPredicate(int categoryId)
		{
			return p => p.ProductCategories != null && p.ProductCategories.Any(pc => pc.CategoryId == categoryId);
		}

		public IEnumerable<Product> GetProductsByCategory(int categoryId, int offset = 0, int limit = 0)
		{
			var predicate = GetProductsByCategoryPredicate(categoryId);
			return GetAll(predicate, includeProperties: _properties, offset: offset, limit: limit);
		}

		public int GetProductsCount(int categoryId = 0)
		{
			if (categoryId > 0)
			{
				var predicate = GetProductsByCategoryPredicate(categoryId);
				return Count(predicate);
			}
			
			return Count();
		}
	}
}
