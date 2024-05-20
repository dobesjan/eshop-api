using Eshop.Api.DataAccess.Repository;
using Eshop.Api.Models.Images;
using Eshop.Api.Models.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eshop.Api.BusinessLayer.Services.Products
{
    public interface IProductService : IEshopService
    {
        IEnumerable<Product> GetProducts(int offset = 0, int limit = 0, int categoryId = 0);
        Product GetProduct(int id);
        bool UpsertProduct(Product product);
        bool UpsertProductPrice(ProductPriceList productPriceList);
        bool LinkImageToProduct(int productId, int imageId);
        bool UnlinkImageFromProduct(int productId, int imageId);
        bool AddProductToCategory(ProductCategory category);
    }
}
