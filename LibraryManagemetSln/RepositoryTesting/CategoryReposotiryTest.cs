using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LibraryManagemetApi.Interfaces;
using LibraryManagemetApi.Models;
using LibraryManagemetApi.Exceptions;
using LibraryManagemetApi.Repositories;
using Microsoft.EntityFrameworkCore;
using LibraryManagemetApi.Contexts;

namespace RepositoryTesting
{
    internal class CategoryReposotiryTest
    {
        DbContextOptionsBuilder optionsBuilder;
        IRepository<int, Category> categoryRepo;

        [SetUp]
        public async Task Setup()
        {
            optionsBuilder = new DbContextOptionsBuilder().UseInMemoryDatabase("libraryTest");
            var context = new LibraryManagementContext(optionsBuilder.Options);
            categoryRepo = new CategoryRepository(context);
        }

        [Test]
        public async Task AddCategory()
        {
            var category = new Category { Name = "Category1" };
            var result = await categoryRepo.Insert(category);
            Assert.That(result, Is.EqualTo(category));
        }

        [Test]
        public async Task GetCategoryById()
        {
            var category = new Category { Name = "Category1" };
            await categoryRepo.Insert(category);
            var result = await categoryRepo.GetOneById(category.Id);
            Assert.That(result, Is.EqualTo(category));
        }

        [Test]
        public async Task GetCategoryByName()
        {
            var category = new Category { Name = "Category1" };
            await categoryRepo.Insert(category);
            var result = await ((CategoryRepository)categoryRepo).GetCategoryByName(category.Name);
            Assert.That(result.Name, Is.EqualTo(category.Name));
        }

        [Test]
        public async Task GetAllCategories()
        {
            var category = new Category { Name = "Category1" };
            await categoryRepo.Insert(category);
            var result = await categoryRepo.Get();
            Assert.That(result, Is.Not.Empty);
        }

        [Test]
        public async Task UpdateCategory()
        {
            var category = new Category { Name = "Category1" };
            await categoryRepo.Insert(category);
            category.Name = "Category2";
            var result = await categoryRepo.Update(category);
            Assert.That(result, Is.EqualTo(category));
        }

        [Test]
        public async Task DeleteCategory()
        {
            var category = new Category { Name = "Category1" };
            await categoryRepo.Insert(category);
            var delval = await categoryRepo.Delete(category.Id);
            Assert.That(delval, Is.EqualTo(category));
        }
        
        [Test]
        public async Task GetCategoryByNameEntityNotFoundException()
        {
            Assert.ThrowsAsync<EntityNotFoundException>(async () => await ((CategoryRepository)categoryRepo).GetCategoryByName(""));
        }
        [Test]
        public async Task DeleteCardThrowsEntityNotFondExcetion()
        {
            Assert.ThrowsAsync<EntityNotFoundException>(() => categoryRepo.Delete(0));
        }

    }
}
