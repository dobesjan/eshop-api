using Eshop.Api.Models.Interfaces;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eshop.Api.Models.Products
{
	public class Category : Entity
	{
		[DatabaseGenerated(DatabaseGeneratedOption.Identity), Key]
		public int Id { get; set; }

		[Required]
		public string Name { get; set; }

		public int ParentCategoryId { get; set; }

		[ForeignKey(nameof(ParentCategoryId))]
		[ValidateNever]
		public Category ParentCategory { get; set; }

		public bool Enabled { get; set; }

		[ValidateNever]
		public List<ProductCategory> ProductCategories { get; set; }
	}
}
