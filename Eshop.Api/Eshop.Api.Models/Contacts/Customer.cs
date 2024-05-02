using Eshop.Api.Models.Orders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eshop.Api.Models.Contacts
{
    public class Customer : Contact
    {
        public bool NewsletterAgree { get; set; }

		public List<OrderCustomer> CustomerContacts { get; set; }
	}
}
