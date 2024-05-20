using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eshop.Api.Models.Currencies
{
	public class CurrencyPreference : Entity
	{
		public string? Token { get; set; }
		public int? UserId { get; set; }

		public int CurrencyId { get; set; }

		[ForeignKey(nameof(CurrencyId))]
		[ValidateNever]
		public Currency Currency { get; set; }
	}
}
