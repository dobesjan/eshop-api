using Eshop.Api.Models.Interfaces;
using Eshop.Api.Models.Orders;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Eshop.Api.Models.Products
{
	public class Product : Entity
	{
		[Required]
		public string Name { get; set; }

		public bool Enabled { get; set; }

		public bool IsInStock { get; set; }

		public int ViewOrder { get; set; }

		public int BuyLimit { get; set; }

		[ValidateNever]
		public List<ProductCategory> ProductCategories { get; set; }

		[ValidateNever]
		public List<OrderProduct> OrderProducts { get; set; }

		[ValidateNever]
		public List<ProductImage> ProductImages { get; set; }

		[ValidateNever]
		[InverseProperty(nameof(Product))]
		public List<ProductPriceList>? ProductPrices { get; set; }

		public override object ToJson()
		{
			return new
			{
				Id = Id,
				Name = Name,
				Enabled = Enabled,
				IsInStock = IsInStock,
				BuyLimit = BuyLimit,
				Categories = ProductCategories != null && ProductCategories.Any() ? ProductCategories.Select(pc => pc.Category).ToList().Select(cc => new { Id = cc.Id, Name = cc.Name, Enabled = cc.Enabled }) : null,
				Images = ProductImages != null && ProductImages.Any() ? ProductImages.Select(pi => pi.Image).ToList().Select(i => new { Id = i.Id, FileName = i.FileName }) : null,
				Prices = ProductPrices != null && ProductPrices.Any() ? ProductPrices.ToList().Select(pr => new { Id = pr.Id, Cost = pr.Cost, CostWithTax = pr.CostWithTax, CostBefore = pr.CostBefore, Currency = pr.Currency }) : null
			};
		}
	}
}
