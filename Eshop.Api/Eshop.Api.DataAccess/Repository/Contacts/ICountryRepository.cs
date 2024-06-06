using Eshop.Api.Models.Contacts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eshop.Api.DataAccess.Repository.Contacts
{
    public interface ICountryRepository : IRepository<Country>
    {
        IEnumerable<Country> GetCountries(bool enabled=true);
        Country GetCountry(int id, bool enabled=true);
    }
}
