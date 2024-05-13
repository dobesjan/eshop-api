﻿using Eshop.Api.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eshop.Api.Models.Orders
{
	public class OrderShipping : Entity
	{
		public int OrderId { get; set; }
		public Order Order { get; set; }

		public int ShippingId { get; set; }
		public Shipping Shipping { get; set; }
	}
}