using Eshop.Api.Models.Contacts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Eshop.Api.Models.ViewModels.Contacts
{
	public class CustomerVM
	{
		public int UserId { get; set; }
		public string Token { get; set; }
		public Customer Customer { get; set; }
	}
}
