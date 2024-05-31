using Eshop.Api.BusinessLayer.Services.Products;
using Microsoft.AspNetCore.Mvc;

namespace Eshop.UI.Components
{
    public class CategoryNavbarViewComponent : ViewComponent
    {
        private readonly ICategoryService _categoryService;

        public CategoryNavbarViewComponent(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        public IViewComponentResult Invoke()
        {
            var categories = _categoryService.GetCategoryHierarchy();
            return View(categories);
        }
    }
}
