using Eshop.Api.Models.Contacts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eshop.Api.Models.ViewModels.Contacts
{
    public class AddressVM
    {
        public string Token { get; set; }
        public Address Address { get; set; }
    }
}
