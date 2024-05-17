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

		// True when customer send order
		public bool IsOrdered { get; set; }

		public DateTime CreatedDate { get; set; }

		public List<OrderProduct> OrderProducts { get; set; }

		public List<OrderShipping> OrderShipping { get; set; }

		//TODO: Consider if we need multiple customers for one order
		public List<OrderCustomer> CustomerContacts { get; set; }

		public int AddressId { get; set; }

		[ForeignKey(nameof(AddressId))]
		[ValidateNever]
		public Address DeliveryAddress { get; set; }

		//DateTime when first products were put to cart
		public DateTime Created { get; set; }

		//DateTime when customer sent order
		public DateTime? SentTime { get; set; }

		//DateTime when order is delivered to customer
		public DateTime? DeliveryTime { get; set; }

		public double CalculateTotalCost(int currency, bool isWithTax = false)
		{
			if (OrderProducts != null)
			{
				return Math.Round(OrderProducts.Where(p => p.Product != null).Sum(a => a.Product.GetPrice(currency, isWithTax)));
			}
			return 0;
		}

		public override bool IsValid()
		{
			if (UserId <= 0 || Token == String.Empty) return false;
			if (OrderProducts == null) return false;
			if (!OrderProducts.Any()) return false;
			if (CustomerContacts == null) return false;
			if (!CustomerContacts.Any()) return false;

			return true;
		}
	}
}
