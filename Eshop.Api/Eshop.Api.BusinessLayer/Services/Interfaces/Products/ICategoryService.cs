﻿using Eshop.Api.Models.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eshop.Api.BusinessLayer.Services.Interfaces.Products
{
    public interface ICategoryService : IEshopService
    {
        IEnumerable<Category> GetCategories();
        Category GetCategory(int id);
        bool UpsertCategory(Category category);
    }
}
