using Eshop.Api.Models.Contacts;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Eshop.UI.Models
{
    public class ContactVM
    {
        public Contact BillingContact { get; set; }
        public Address DeliveryAddress { get; set; }
        [ValidateNever]
        public IEnumerable<SelectListItem> Countries { get; set; }
    }
}
