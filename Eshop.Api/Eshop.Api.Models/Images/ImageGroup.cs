using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eshop.Api.Models.Images
{
	public class ImageGroup : Entity
	{
		[Required]
		public string Name { get; set; }
	}
}
