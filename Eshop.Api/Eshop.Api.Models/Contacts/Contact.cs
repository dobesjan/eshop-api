using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eshop.Api.Models.Contacts
{
	public class Contact : Entity
	{
		public int PersonId { get; set; }

		[ForeignKey(nameof(PersonId))]
		[ValidateNever]
		public Person Person { get; set; }

		public int? AddressId { get; set; }

		[ForeignKey(nameof(AddressId))]
		[ValidateNever]
		public Address Address { get; set; }
	}
}
