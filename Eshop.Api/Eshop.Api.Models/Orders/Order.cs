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
		public int? UserId { get; set; }

		public string Token { get; set; }

		public int OrderStatusId { get; set; }

		[ForeignKey(nameof(OrderStatusId))]
		[ValidateNever]
		public OrderStatus? OrderStatus { get; set; }

		// True when customer send order
		public bool IsOrdered { get; set; }

		public DateTime CreatedDate { get; set; }

		public List<OrderProduct>? OrderProducts { get; set; }

		public int? ShippingId { get; set; }

		[ForeignKey(nameof(ShippingId))]
		[ValidateNever]
		public Shipping? Shipping { get; set; }

		public int? CustomerId { get; set; }

		[ForeignKey(nameof(CustomerId))]
		[ValidateNever]
		public Customer? Customer { get; set; }

		public int? AddressId { get; set; }

		[ForeignKey(nameof(AddressId))]
		[ValidateNever]
		public Address? DeliveryAddress { get; set; }

		[InverseProperty(nameof(Order))]
		public Payment? Payment { get; set; }

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

		public override bool Validate()
		{
			if (UserId <= 0 && Token == String.Empty)
			{
				throw new InvalidDataException("UserId or token not provided!");
			}
			/*
			if (OrderProducts == null)
			{
				throw new InvalidDataException("There are not any products in order!");
			}

			if (OrderProducts.Any())
			{
				throw new InvalidDataException("There are not any products in order!");
			}
			*/

			return true;
		}

		public bool IsReadyToSend()
		{
			Validate();

			if (Customer == null) throw new InvalidDataException("Customer data not filled");
			if (Customer.Person == null) throw new InvalidDataException("Customer personal data not filled");
			if (Customer.Address == null) throw new InvalidDataException("Customer address not filled");

			Customer.Person.Validate();
			Customer.Address.Validate();

			return true;
		}
	}
}
