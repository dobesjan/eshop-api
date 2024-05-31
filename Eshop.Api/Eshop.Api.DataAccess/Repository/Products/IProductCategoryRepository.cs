using Eshop.Api.Models.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eshop.Api.DataAccess.Repository.Products
{
    public interface IProductCategoryRepository : IRepository<ProductCategory>
    {
        ProductCategory GetProductCategory(int productId, int categoryId);
        IEnumerable<ProductCategory> GetCategoriesForProduct(int productId);
    }
}
