using Eshop.Api.Models.Orders;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eshop.Api.Models.Contacts
{
	public class Person : Entity
	{
		public string? FirstName { get; set; }

		public string? LastName { get; set; }

		public string? Email { get; set; }

		public string? PhoneNumber { get; set; }

		public override bool Validate()
		{
			if (String.IsNullOrEmpty(FirstName)) throw new InvalidDataException("First name not provided");
			if (String.IsNullOrEmpty(LastName)) throw new InvalidDataException("Last name not provided");
			if (String.IsNullOrEmpty(Email)) throw new InvalidDataException("Email name not provided");
			if (String.IsNullOrEmpty(PhoneNumber)) throw new InvalidDataException("Phone number name not provided");

			return true;
		}
	}
}
