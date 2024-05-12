using Eshop.Api.Models.Contacts;
using Eshop.Api.Models.Interfaces;
using Eshop.Api.Models.Products;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eshop.Api.Models.Orders
{
	public class Order : Entity
	{
		public int UserId { get; set; }

		public string Token { get; set; }

		public int OrderStatusId { get; set; }

		[ForeignKey(nameof(OrderStatusId))]
		[ValidateNever]
		public OrderStatus OrderStatus { get; set; }

		public DateTime CreatedDate { get; set; }

		public List<OrderProduct> OrderProducts { get; set; }

		public List<OrderShipping> OrderShipping { get; set; }

		//TODO: Consider if we need multiple customers for one order
		public List<OrderCustomer> CustomerContacts { get; set; }

		public int AddressId { get; set; }

		[ForeignKey(nameof(AddressId))]
		[ValidateNever]
		public Address DeliveryAddress { get; set; }
	}
}
