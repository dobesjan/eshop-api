using Eshop.Api.DataAccess.Repository;
using Eshop.Api.DataAccess.UnitOfWork;
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
		private IUnitOfWork _unitOfWork;

		public CategoryService(IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;
		}

		public IEnumerable<Category> GetCategories()
		{
			return _unitOfWork.CategoryRepository.GetAll();
		}

		public Category GetCategory(int id)
		{
			if (id <= 0)
			{
				throw new InvalidDataException($"Wrong value: {id} for category id!");
			}

			var category = _unitOfWork.CategoryRepository.Get(id, includeProperties: "ParentCategory");
			if (category == null)
			{
				throw new InvalidDataException($"Category with id: {id} not found in db!");
			}

			return category;
		}

		public bool UpsertCategory(Category category)
		{
			return UpsertEntity(category, _unitOfWork.CategoryRepository) != null;
		}
	}
}
