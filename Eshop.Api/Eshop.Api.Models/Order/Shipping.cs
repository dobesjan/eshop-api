using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eshop.Api.Models.Order
{
	public class Shipping
	{
		[Key]
		public int Id { get; set; }

		[Required]
		public string Name { get; set; }

		public bool Enabled { get; set; }

		public List<OrderShipping> OrderShipping { get; set; }
	}
}
