using Eshop.Api.Models.Products;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eshop.Api.Models.Images
{
	public class Image : Entity
	{
		public string FileName { get; set; }

		public int ImageGroupId { get; set; }

		[ForeignKey(nameof(ImageGroupId))]
		[ValidateNever]
		public ImageGroup ImageGroup { get; set; }

		[ValidateNever]
		public List<ProductImage> ProductImages { get; set; }
	}
}
