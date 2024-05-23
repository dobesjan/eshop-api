using Eshop.Api.Models.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eshop.Api.DataAccess.Repository.Products
{
	public interface IProductImageRepository : IRepository<ProductImage>
	{
		ProductImage Get(int productId, int imageId);
	}
}
