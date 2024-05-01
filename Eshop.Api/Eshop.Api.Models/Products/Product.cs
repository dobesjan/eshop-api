using Eshop.Api.Models.Interfaces;
using Eshop.Api.Models.Order;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eshop.Api.Models.Products
{
	public class Product : Entity
	{
		[Key]
		public int Id { get; set; }

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
	}
}
