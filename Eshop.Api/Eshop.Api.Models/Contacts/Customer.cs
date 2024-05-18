using Eshop.Api.Models.Orders;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eshop.Api.Models.Contacts
{
    public class Customer : Contact
    {
        public bool NewsletterAgree { get; set; }
	}
}
