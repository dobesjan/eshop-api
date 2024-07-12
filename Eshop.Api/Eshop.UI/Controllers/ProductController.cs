using Eshop.Api.BusinessLayer.Services.Contacts;
using Eshop.Api.BusinessLayer.Services.Orders;
using Eshop.Api.BusinessLayer.Services.Products;
using Eshop.Api.Models.Orders;
using Eshop.Api.Models.Products;
using Eshop.UI.Models;
using Microsoft.AspNetCore.Mvc;

namespace Eshop.UI.Controllers
{
    public class ProductController : EshopBaseController
    {
        private readonly ILogger<ProductController> _logger;
        private readonly ICategoryService _categoryService;
        private readonly IProductService _productService;
        private readonly IOrderService _orderService;

        public ProductController(ILogger<ProductController> logger, ICategoryService categoryService, IProductService productService, ICustomerService customerService, IOrderService orderService) : base(customerService, logger)
        {
            _logger = logger;
            _categoryService = categoryService;
            _productService = productService;
            _orderService = orderService;
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
                RedirectToRoute("/");
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

        [HttpPost]
        public IActionResult ToCart(int? productId, int count = 1)
        {
            return HandleResponse(() =>
            {
                if (!productId.HasValue) return Redirect("/");

                var customer = GetCustomer();

                _orderService.AddProductToOrder(productId.Value, customer.Id, count);

                // Get the referer URL
                var refererUrl = Request.Headers["Referer"].ToString();
                if (string.IsNullOrEmpty(refererUrl))
                {
                    // Fallback to a default URL if referer is not available
                    refererUrl = "/";
                }

                return Redirect(refererUrl);
            }, Redirect("/"));
        }
    }
}
