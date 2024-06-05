using Eshop.Api.Models.Contacts;
using Eshop.Api.Models.Currencies;
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
using System.Xml.Linq;

namespace Eshop.Api.Models.Orders
{
	public class Order : Entity
	{
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

		//TODO: Figure out how to correct contacts linking - currently if customer changes something (like address, etc.)
		// then it's updated in order as well, which means that historical data will be changed unexpectly
		// potentially add contact relation (new contact entity will be created with each new order)
		public int? CustomerId { get; set; }

		[ForeignKey(nameof(CustomerId))]
		[ValidateNever]
		public Customer? Customer { get; set; }

        public int? BillingAddressId { get; set; }

        [ForeignKey(nameof(BillingAddressId))]
        [ValidateNever]
        public Address? BillingAddress { get; set; }

        public int? AddressId { get; set; }

		[ForeignKey(nameof(AddressId))]
		[ValidateNever]
		public Address? DeliveryAddress { get; set; }

		[InverseProperty(nameof(Order))]
		public Payment? Payment { get; set; }

		//DateTime when customer sent order
		public DateTime? SentTime { get; set; }

		//DateTime when order is delivered to customer
		public DateTime? DeliveryTime { get; set; }

		public int CurrencyId { get; set; }

		[ForeignKey(nameof(CurrencyId))]
		[ValidateNever]
		public Currency Currency { get; set; }

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
			if (BillingAddress == null) throw new InvalidDataException("Billing address not filled");

			Customer.Person.Validate();
            BillingAddress.Validate();

			return true;
		}

		public override object ToJson()
		{
			return new
			{
				Id = Id,
				OrderStatusId = OrderStatusId,
				OrderStatus = OrderStatus,
				IsOrdered = IsOrdered,
				CreatedDate = CreatedDate,
				//OrderProducts = OrderProducts != null && OrderProducts.Any() ? OrderProducts.Select(op => new { Id = op.ProductId, Name = op.Product.Name, IsInStock = op.Product.IsInStock, Count = op.Count }) : null,
				OrderProducts = OrderProducts != null && OrderProducts.Any() ? OrderProducts.Select(op => op.ToJson()) : null,
				ShippingId = ShippingId,
				Shipping = Shipping,
				CustomerId = CustomerId,
				Customer = Customer,
				AddressId = AddressId,
				DeliveryAddress = DeliveryAddress,
				Payment = Payment != null ? new { PaymentStatus = Payment.PaymentStatus, PaymentMethod = Payment.PaymentMethod, Cost = Payment.Cost, CostWithTax = Payment.CostWithTax } : null,
				SentTime = SentTime,
				DeliveryTime = DeliveryTime,
				Currency = Currency
			};
		}
	}
}
