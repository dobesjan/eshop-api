using Eshop.Api.Models.Images;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eshop.Api.Models.Products
{
	public class ProductImage : Entity
	{
		public int ProductId { get; set; }
		[ValidateNever]
		public Product Product { get; set; }

		public int ImageId { get; set; }
		[ValidateNever]
		public Image Image { get; set; }
	}
}
