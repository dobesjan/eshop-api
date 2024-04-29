using Eshop.Api.Models.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eshop.Api.Models.Order
{
	public class OrderProduct
	{
		public int ProductId { get; set; }
		public Product Product { get; set; }

		public int OrderId { get; set; }
		public Order Order { get; set; }
	}
}
