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

		public IEnumerable<Category> GetCategoriesForProduct(int productId)
		{
			var relations = _unitOfWork.ProductCategoryRepository.GetCategoriesForProduct(productId);

			if (relations == null) throw new InvalidDataException($"There are not any categories for product with id: {productId}!");

			return relations.Select(r => r.Category).ToList();
        }


        public IEnumerable<Category> GetCategoryHierarchy()
		{
			var categories = GetCategories().Where(c => c.Enabled).ToList();
			return BuildHierarchy(categories, null);
		}

		private IEnumerable<Category> BuildHierarchy(List<Category> categories, int? parentId)
		{
			return categories
				.Where(c => c.ParentCategoryId == parentId)
				.Select(c => new Category
				{
					Id = c.Id,
					Name = c.Name,
					ParentCategoryId = c.ParentCategoryId,
					Enabled = c.Enabled,
					ProductCategories = c.ProductCategories,
					ParentCategory = c.ParentCategory,
					Children = BuildHierarchy(categories, c.Id).ToList()
				})
				.ToList();
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
