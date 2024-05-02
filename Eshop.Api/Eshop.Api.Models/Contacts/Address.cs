using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Eshop.Api.Models.Orders;
using System.ComponentModel.DataAnnotations;

namespace Eshop.Api.Models.Contacts
{
    public class Address : Entity
    {
		public int AddressTypeId { get; set; }

		[ForeignKey(nameof(AddressTypeId))]
		[ValidateNever]
		public AddressType AddressType { get; set; }

		public int UserId { get; set; }

		[Required]
		public string Street { get; set; }

		[Required]
		public string City { get; set; }

		[Required]
		public string PostalCode { get; set; }

		[Required]
		public string Country { get; set; }
	}
}
