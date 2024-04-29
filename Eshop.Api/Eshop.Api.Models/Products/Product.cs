using Eshop.Api.Models.Order;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eshop.Api.Models.Products
{
	public class Product
	{
		[Key]
		public int Id { get; set; }

		[Required]
		public string Name { get; set; }

		public bool Enabled { get; set; }

		public bool IsInStock { get; set; }

		public int ViewOrder { get; set; }

		public int BuyLimit { get; set; }

		public List<ProductCategory> ProductCategories { get; set; }

		public List<OrderProduct> OrderProducts { get; set; }
	}
}
