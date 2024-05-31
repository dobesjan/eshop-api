using Eshop.Api.DataAccess.Data;
using Eshop.Api.Models.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eshop.Api.DataAccess.Repository.Products
{
    public class ProductCategoryRepository : Repository<ProductCategory>, IProductCategoryRepository
    {
        private readonly string _properties = "Product,Category";

        public ProductCategoryRepository(ApplicationDbContext db) : base(db)
        {
        }

        public ProductCategory GetProductCategory(int productId, int categoryId)
        {
            return Get(pc => pc.ProductId == productId && pc.CategoryId == categoryId, includeProperties: _properties);
        }

        public IEnumerable<ProductCategory> GetCategoriesForProduct(int productId)
        {
            return GetAll(pc => pc.ProductId == productId, includeProperties: _properties);
        }
    }
}
