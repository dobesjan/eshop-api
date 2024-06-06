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
    public class Customer : Entity
    {
        public bool NewsletterAgree { get; set; }
        public bool IsLogged { get; set; }

        public string UserId { get; set; }

        public string Token { get; set; }

        public int ContactId { get; set; }

        [ForeignKey(nameof(ContactId))]
        [ValidateNever]
        public Contact? Contact { get; set; }
    }
}
