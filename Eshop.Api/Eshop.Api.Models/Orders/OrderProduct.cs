using Eshop.Api.Models.Interfaces;
using Eshop.Api.Models.Products;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
		public Product Product { get; set; }

		public int OrderId { get; set; }
		public Order Order { get; set; }

		public int Count { get; set; }

		public override bool Validate()
		{
			if (ProductId <= 0) throw new InvalidDataException("Wrong value for productId");
			if (OrderId <= 0) throw new InvalidDataException("Wrong value for orderId");
			if (Count <= 0) throw new InvalidDataException("Wrong value for count");

			return true;
		}
	}
}
