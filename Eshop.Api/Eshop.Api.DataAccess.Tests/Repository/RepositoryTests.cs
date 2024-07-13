using Eshop.Api.DataAccess.Data;
using Eshop.Api.DataAccess.Repository;
using Eshop.Api.Models.Products;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eshop.Api.DataAccess.Tests.Repository
{
	[TestFixture]
	public class CategoryRepositoryTests
	{
		private ApplicationDbContext _context;
		private Repository<Category> _repository;

		[SetUp]
		public void SetUp()
		{
			var options = new DbContextOptionsBuilder<ApplicationDbContext>()
				.UseInMemoryDatabase(databaseName: "TestDatabase")
				.Options;
			_context = new ApplicationDbContext(options);
			_repository = new Repository<Category>(_context);
		}

		[TearDown]
		public void TearDown()
		{
			_context.Database.EnsureDeleted();
			_context.Dispose();
		}

		[Test]
		public void Add_AddsEntityToDbSet()
		{
			var category = new Category { Id = 1, Name = "Electronics" };

			_repository.Add(category);
			_repository.Save();

			var result = _context.Categories.Find(1);
			Assert.IsNotNull(result);
			Assert.That(result.Name, Is.EqualTo("Electronics"));
		}

		[Test]
		public void GetAll_ReturnsAllEntities()
		{
			var categories = new List<Category>
			{
				new Category { Id = 1, Name = "Electronics" },
				new Category { Id = 2, Name = "Clothing" }
			};
			_context.Categories.AddRange(categories);
			_context.SaveChanges();

			var result = _repository.GetAll().ToList();

			Assert.That(result.Count, Is.EqualTo(2));
			Assert.That(result[0].Name, Is.EqualTo("Electronics"));
			Assert.That(result[1].Name, Is.EqualTo("Clothing"));
		}

		[Test]
		public void Get_ReturnsEntityById()
		{
			var category = new Category { Id = 1, Name = "Electronics" };
			_context.Categories.Add(category);
			_context.SaveChanges();

			var result = _repository.Get(1);

			Assert.IsNotNull(result);
			Assert.That(result.Name, Is.EqualTo("Electronics"));
		}

		[Test]
		public void Update_UpdatesEntityInDbSet()
		{
			var category = new Category { Id = 1, Name = "Electronics" };
			_context.Categories.Add(category);
			_context.SaveChanges();

			category.Name = "Updated Electronics";
			_repository.Update(category);
			_repository.Save();

			var result = _context.Categories.Find(1);
			Assert.That(result.Name, Is.EqualTo("Updated Electronics"));
		}

		[Test]
		public void Remove_RemovesEntityFromDbSet()
		{
			var category = new Category { Id = 1, Name = "Electronics" };
			_context.Categories.Add(category);
			_context.SaveChanges();

			_repository.Remove(category);
			_repository.Save();

			var result = _context.Categories.Find(1);
			Assert.IsNull(result);
		}

		[Test]
		public void Count_ReturnsNumberOfEntities()
		{
			var categories = new List<Category>
			{
				new Category { Id = 1, Name = "Electronics" },
				new Category { Id = 2, Name = "Clothing" }
			};
			_context.Categories.AddRange(categories);
			_context.SaveChanges();

			var result = _repository.Count();

			Assert.That(result, Is.EqualTo(2));
		}

		[Test]
		public void IsStored_ReturnsTrueIfEntityExists()
		{
			var category = new Category { Id = 1, Name = "Electronics" };
			_context.Categories.Add(category);
			_context.SaveChanges();

			var result = _repository.IsStored(1);

			Assert.IsTrue(result);
		}

		[Test]
		public void Save_CallsSaveChangesOnContext()
		{
			var category = new Category { Id = 1, Name = "Electronics" };

			_repository.Add(category);
			_repository.Save();

			var result = _context.Categories.Find(1);
			Assert.IsNotNull(result);
		}
	}
}
