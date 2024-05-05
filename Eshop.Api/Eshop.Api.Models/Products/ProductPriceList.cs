using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eshop.Api.Models.Products
{
	public class ProductPriceList : Entity
	{
		public double Cost { get; set; }
		public double CostWithTax { get; set; }
		public double CostBefore { get; set; }

		public int ProductId { get; set; }

		[ForeignKey(nameof(ProductId))]
		[ValidateNever]
		public Product Product { get; set; }
	}
}
