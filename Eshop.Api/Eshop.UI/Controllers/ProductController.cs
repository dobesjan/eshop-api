using Eshop.Api.BusinessLayer.Services.Products;
using Eshop.Api.Models.Products;
using Eshop.UI.Models;
using Microsoft.AspNetCore.Mvc;

namespace Eshop.UI.Controllers
{
    public class ProductController : Controller
    {
        private readonly ILogger<ProductController> _logger;
        private readonly ICategoryService _categoryService;
        private readonly IProductService _productService;

        public ProductController(ILogger<ProductController> logger, ICategoryService categoryService, IProductService productService)
        {
            _logger = logger;
            _categoryService = categoryService;
            _productService = productService;
        }

        public IActionResult Index(int? id)
        {
            if (!id.HasValue)
            {
                RedirectToRoute("/");
            }

            Product product = null;
            try
            {
                product = _productService.GetProduct(id.Value);
            }
            catch (InvalidDataException ex)
            {
                _logger.LogInformation(ex.Message);
            }

            var vm = new ProductVM
            {
                Categories = null,
                Product = product
            };

            List<Category> category = null;
            try
            {
                category = _categoryService.GetCategoriesForProduct(id.Value).ToList();
                vm.Categories = category;
            }
            catch (InvalidDataException ex)
            {
                _logger.LogInformation(ex.Message);
                RedirectToRoute("/");
            }

            return View(vm);
        }
    }
}
