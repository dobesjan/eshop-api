using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eshop.Api.Models.Orders
{
	public class OrderFilter
	{
		public int? OrderId { get; set; }
		public string OrderStatus { get; set; }
		public bool? IsOrdered { get; set; }
		public DateTime? CreatedDateFrom { get; set; }
		public DateTime? CreatedDateTo { get; set; }
		public DateTime? SentTimeFrom { get; set; }
		public DateTime? SentTimeTo { get; set; }
		public DateTime? DeliveryTimeFrom { get; set; }
		public DateTime? DeliveryTimeTo { get; set; }
		public string Shipping { get; set; }
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public string Email { get; set; }
		public string PhoneNumber { get; set; }
		public string Street { get; set; }
		public string City { get; set; }
		public string PostalCode { get; set; }
		public string Country { get; set; }
		public bool? NewsletterAgree { get; set; }
	}
}
