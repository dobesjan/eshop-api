using Eshop.Api.Models.Products;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eshop.Api.Models.Order
{
	public class Order
	{
		[Key]
		public int Id { get; set; }

		public int UserId { get; set; }

		public string Token { get; set; }

		public int OrderStatusId { get; set; }

		[ForeignKey(nameof(OrderStatusId))]
		[ValidateNever]
		public OrderStatus OrderStatus { get; set; }

		public DateTime CreatedDate { get; set; }

		public List<OrderProduct> OrderProducts { get; set; }

		public List<OrderShipping> OrderShipping { get; set; }
	}
}
