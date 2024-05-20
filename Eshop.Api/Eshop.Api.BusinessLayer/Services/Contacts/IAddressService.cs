using Eshop.Api.Models.Contacts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eshop.Api.BusinessLayer.Services.Contacts
{
    public interface IAddressService
    {
        bool SaveAddress(Address address);
    }
}
