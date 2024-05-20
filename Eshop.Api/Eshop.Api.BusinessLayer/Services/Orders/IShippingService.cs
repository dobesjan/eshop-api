using Eshop.Api.Models.Orders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eshop.Api.BusinessLayer.Services.Orders
{
    public interface IShippingService : IEshopService
    {
        Shipping GetShipping(int shippingId = 0);
        IEnumerable<Shipping> GetShippings(bool enabled = true);
    }
}
