using Eshop.Api.DataAccess.Data;
using Eshop.Api.Models.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eshop.Api.DataAccess.Repository.Products
{
	public class ProductImageRepository : Repository<ProductImage>, IProductImageRepository
	{
		public ProductImageRepository(ApplicationDbContext db) : base(db)
		{
		}

		public ProductImage Get(int productId, int imageId)
		{
			return Get(pi => pi.ProductId == productId && pi.ImageId == imageId);
		}
	}
}
