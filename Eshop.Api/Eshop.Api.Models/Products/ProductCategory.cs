using Eshop.Api.Models.Interfaces;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eshop.Api.Models.Products
{
	public class ProductCategory : Entity
	{
		[Key]
		public int Id { get; set; }

		public int ProductId { get; set; }
		[ValidateNever]
		public Product Product { get; set; }

		public int CategoryId { get; set; }
		[ValidateNever]
		public Category Category { get; set; }
	}
}
