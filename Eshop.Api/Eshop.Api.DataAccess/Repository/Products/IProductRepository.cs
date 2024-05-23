using Eshop.Api.Models.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eshop.Api.DataAccess.Repository.Products
{
	public interface IProductRepository : IRepository<Product>
	{
		IEnumerable<Product> GetProductsByCategory(int categoryId, int offset = 0, int limit = 0);
		int GetProductsCount(int categoryId = 0);
		IEnumerable<Product> GetProducts(int offset = 0, int limit = 0);
		Product GetProduct(int productId);
	}
}
