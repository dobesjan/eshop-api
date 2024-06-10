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
		public int CustomerId { get; set; }

		[Required]
		public string Street { get; set; }

		[Required]
		public string City { get; set; }

		[Required]
		public string PostalCode { get; set; }

		public int CountryId { get; set; }

        [ForeignKey(nameof(CountryId))]
        [ValidateNever]
        public Country Country { get; set; }

		public override bool Validate()
		{
			if (String.IsNullOrEmpty(Street)) throw new InvalidDataException("Street not provided");
			if (String.IsNullOrEmpty(City)) throw new InvalidDataException("City not provided");
			if (String.IsNullOrEmpty(PostalCode)) throw new InvalidDataException("Postal code not provided");
			if (CountryId <= 0) throw new InvalidDataException("Country not provided");

			return true;
		}
	}
}
