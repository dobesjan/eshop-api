using Eshop.Api.Models.Interfaces;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eshop.Api.Models.Orders
{
	public class PaymentMethod : Entity
	{
		[Required]
		public string Name { get; set; }

		[ValidateNever]
		public List<ShippingPaymentMethod> ShippingPaymentMethod { get; set; }
	}
}
