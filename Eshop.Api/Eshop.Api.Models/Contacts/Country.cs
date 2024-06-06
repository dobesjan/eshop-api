using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eshop.Api.Models.Contacts
{
    public class Country : Entity
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string Acronym { get; set; }

        public bool IsEnabled { get; set; }
    }
}
