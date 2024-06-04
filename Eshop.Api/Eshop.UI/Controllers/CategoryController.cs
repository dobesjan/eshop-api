using Eshop.Api.BusinessLayer.Services.Products;
using Eshop.Api.Models.Products;
using Eshop.UI.Models;
using Microsoft.AspNetCore.Mvc;

namespace Eshop.UI.Controllers
{
    public class CategoryController : EshopBaseController
    {
        private readonly ILogger<CategoryController> _logger;
        private readonly ICategoryService _categoryService;
        private readonly IProductService _productService;

        public CategoryController(ILogger<CategoryController> logger, ICategoryService categoryService, IProductService productService)
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

            Category category = null;
            try
            {
                category = _categoryService.GetCategory(id.Value);
            }
            catch (InvalidDataException ex) 
            {
                _logger.LogInformation(ex.Message);
                RedirectToRoute("/");
            }

            var vm = new CategoryVM
            {
                Category = category,
                Products = null
            };

            try
            {
                var products = _productService.GetProducts(categoryId: id.Value);
                vm.Products = products.ToList();
            }
            catch (InvalidDataException ex)
            {
                _logger.LogInformation(ex.Message);
            }

            return View(vm);
        }
    }
}
