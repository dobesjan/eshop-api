using Eshop.Api.Models.Products;

namespace Eshop.UI.Models
{
    public class CategoryVM
    {
        public Category? Category { get; set; }
        public List<Product>? Products { get; set; }
    }
}
