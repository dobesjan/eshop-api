using Eshop.Api.Models.Interfaces;
using Eshop.Api.Models.Orders;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eshop.Api.Models.Products
{
	public class Product : Entity
	{
		[Required]
		public string Name { get; set; }

		public bool Enabled { get; set; }

		public bool IsInStock { get; set; }

		public int ViewOrder { get; set; }

		public int BuyLimit { get; set; }

		[ValidateNever]
		public List<ProductCategory> ProductCategories { get; set; }

		[ValidateNever]
		public List<OrderProduct> OrderProducts { get; set; }

		[ValidateNever]
		public List<ProductImage> ProductImages { get; set; }

		[ValidateNever]
		[InverseProperty(nameof(Product))]
		public List<ProductPriceList>? ProductPrices { get; set; }
	}
}
