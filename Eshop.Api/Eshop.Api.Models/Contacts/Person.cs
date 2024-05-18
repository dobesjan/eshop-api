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
		[Required]
		public string FirstName { get; set; }

		[Required]
		public string LastName { get; set; }

		[Required]
		public string Email { get; set; }

		[Required]
		public string PhoneNumber { get; set; }

		public override bool Validate()
		{
			if (FirstName == String.Empty) throw new InvalidDataException("First name not provided");
			if (LastName == String.Empty) throw new InvalidDataException("Last name not provided");
			if (Email == String.Empty) throw new InvalidDataException("Email name not provided");
			if (PhoneNumber == String.Empty) throw new InvalidDataException("Phone number name not provided");

			return true;
		}
	}
}
