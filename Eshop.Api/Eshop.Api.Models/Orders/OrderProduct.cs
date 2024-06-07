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

		public double Cost { get; set; }
		public double CostWithTax { get; set; }
		public double CostBefore { get; set; }

		[NotMapped]
		[ValidateNever]
		public string CostString
		{
			get
			{
				return GetCostWithCurrencyAcronym(Cost);
			}
		}

        [NotMapped]
        [ValidateNever]
        public string CostWithTaxString
        {
            get
            {
                return GetCostWithCurrencyAcronym(CostWithTax);
            }
        }

        [NotMapped]
        [ValidateNever]
        public string CostBeforeString
        {
            get
            {
                return GetCostWithCurrencyAcronym(CostBefore);
            }
        }

        public override bool Validate()
		{
			if (ProductId <= 0) throw new InvalidDataException("Wrong value for productId");
			if (OrderId <= 0) throw new InvalidDataException("Wrong value for orderId");
			if (Count <= 0) throw new InvalidDataException("Wrong value for count");

			return true;
		}

		private void RecalculateCosts()
		{
			if (Product != null && Order.Currency != null)
			{
				var productPrice = Product.ProductPrices?.FirstOrDefault(p => p.Currency.Id == Order.CurrencyId);
				if (productPrice != null)
				{
					Cost = productPrice.Cost * Count;
					CostWithTax = productPrice.CostWithTax * Count;
					CostBefore = productPrice.CostBefore * Count;
				}
			}
		}

		private string GetCostWithCurrencyAcronym(double cost)
		{
			return $"{cost.ToString()} {Order.Currency.Acronym}";
		}

		public override object ToJson()
		{
			return new
			{
				Product = Product.ToJson(),
				Count = Count,
				Currency = Order.Currency,
				Cost = Cost,
				CostWithTax = CostWithTax,
				CostBefore = CostBefore,
			};
		}
	}
}
