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
	public class ShippingPaymentMethod : Entity
	{
		public int ShippingId { get; set; }
		[ValidateNever]
		public Shipping Shipping { get; set; }

		public int PaymentMethodId { get; set; }
        [ValidateNever]
        public PaymentMethod PaymentMethod { get; set; }
	}
}
