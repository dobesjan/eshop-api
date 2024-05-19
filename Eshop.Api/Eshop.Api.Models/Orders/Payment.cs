﻿using Eshop.Api.Models.Interfaces;
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
	public class Payment : Entity
	{
		public int OrderId { get; set; }

		[ForeignKey(nameof(OrderId))]
		[ValidateNever]
		public Order Order { get; set; }

		public int PaymentStatusId { get; set; }

		[ForeignKey(nameof(PaymentStatusId))]
		[ValidateNever]
		public PaymentStatus PaymentStatus { get; set; }

		public int PaymentMethodId { get; set; }

		[ForeignKey(nameof(PaymentMethodId))]
		[ValidateNever]
		public PaymentMethod PaymentMethod { get; set; }

		public int Cost { get; set; }
		public int CostWithTax { get; set; }
	}
}
