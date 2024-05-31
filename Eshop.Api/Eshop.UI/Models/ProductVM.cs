using Eshop.Api.Models.Products;

namespace Eshop.UI.Models
{
    public class ProductVM
    {
        public List<Category>? Categories { get; set; }
        public Product? Product { get; set; }
    }
}
