using Eshop.Api.BusinessLayer.Services.Interfaces.Products;
using Eshop.Api.DataAccess.Repository.Interfaces;
using Eshop.Api.Models;
using Eshop.Api.Models.Interfaces;
using Eshop.Api.Models.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eshop.Api.BusinessLayer.Services.Products
{
	public class CategoryService : EshopService, ICategoryService
	{
		private IRepository<Category> _categoryRepository;

		public CategoryService(IRepository<Category> categoryRepository)
		{
			_categoryRepository = categoryRepository;
		}

		public IEnumerable<Category> GetCategories()
		{
			return _categoryRepository.GetAll();
		}

		public Category GetCategory(int id)
		{
			if (id <= 0)
			{
				throw new InvalidDataException($"Wrong value: {id} for category id!");
			}

			var category = _categoryRepository.Get(c => c.Id == id, includeProperties: "ParentCategory");
			if (category == null)
			{
				throw new InvalidDataException($"Category with id: {id} not found in db!");
			}

			return category;
		}

		public bool UpsertCategory(Category category)
		{
			return UpsertEntity(category, _categoryRepository);
		}
	}
}
