using Eshop.Api.Models.Currencies;
using Eshop.Api.Models.Interfaces;
using Eshop.Api.Models.Products;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.Metrics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eshop.Api.Models.Orders
{
	public class OrderProduct : Entity
	{
		public int ProductId { get; set; }
		[ValidateNever]
		public Product Product { get; set; }

		public int OrderId { get; set; }
		[ValidateNever]
		public Order Order { get; set; }

		private int _count;
		public int Count
		{
			get => _count;
			set
			{
				_count = value;
				RecalculateCosts();
			}
		}

		public int CurrencyId { get; set; }

		[ForeignKey(nameof(CurrencyId))]
		[ValidateNever]
		public Currency Currency { get; set; }

		public double Cost { get; set; }
		public double CostWithTax { get; set; }
		public double CostBefore { get; set; }

		public override bool Validate()
		{
			if (ProductId <= 0) throw new InvalidDataException("Wrong value for productId");
			if (OrderId <= 0) throw new InvalidDataException("Wrong value for orderId");
			if (Count <= 0) throw new InvalidDataException("Wrong value for count");

			return true;
		}

		private void RecalculateCosts()
		{
			if (Product != null && Currency != null)
			{
				var productPrice = Product.ProductPrices?.FirstOrDefault(p => p.Currency.Id == CurrencyId);
				if (productPrice != null)
				{
					Cost = productPrice.Cost * Count;
					CostWithTax = productPrice.CostWithTax * Count;
					CostBefore = productPrice.CostBefore * Count;
				}
			}
		}

		public override object ToJson()
		{
			return new
			{
				Product = Product.ToJson(),
				Count = Count,
				Currency = Currency,
				Cost = Cost,
				CostWithTax = CostWithTax,
				CostBefore = CostBefore,
			};
		}
	}
}
