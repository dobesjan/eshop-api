using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eshop.Api.Models.Currencies
{
	public class Currency : Entity
	{
		[Required]
		public string Name { get; set; }

		[Required]
		public string Acronym { get; set; }
	}
}
