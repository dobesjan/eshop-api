﻿using Eshop.Api.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eshop.Api.Models.Order
{
	public class PaymentMethod : Entity
	{
		[Key]
		public int Id { get; set; }

		[Required]
		public string Name { get; set; }

		public List<ShippingPaymentMethod> ShippingPaymentMethod { get; set; }
	}
}
