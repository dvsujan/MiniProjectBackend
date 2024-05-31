using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using LibraryManagemetApi.Contexts;
using LibraryManagemetApi.Interfaces;
using LibraryManagemetApi.Models;
using LibraryManagemetApi.Exceptions;
using LibraryManagemetApi.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace RepositoryTesting
{
    internal class AuthorRepositoryTesting
    {
        DbContextOptionsBuilder optionsBuilder; 
        IRepository<int, Author> AuthorRepo;

        [SetUp]
        public async Task Setup()
        {
            optionsBuilder = new DbContextOptionsBuilder().UseInMemoryDatabase("libraryTest");
            var context = new LibraryManagementContext(optionsBuilder.Options);
            AuthorRepo = new AuthorRepository(context);
        }

        [Test]
        public async Task AddAuthor()
        {
            var author = new Author { Name = "Author1", Language = "en" };
            var result = await AuthorRepo.Insert(author);
            Assert.That(result, Is.EqualTo(author));
        }

        [Test]
        public async Task GetAuthorById()
        {
            var author = new Author { Name = "Author1", Language = "en" };
            await AuthorRepo.Insert(author);
            var result = await AuthorRepo.GetOneById(author.Id);
            Assert.That(result, Is.EqualTo(author));
        }
        
        [Test]
        public async Task GetAuthorByName()
        {
            var author = new Author { Name = "Author1", Language = "en" };
            await AuthorRepo.Insert(author);
            var result = await ((AuthorRepository)AuthorRepo).GetAuthorByName(author.Name);
            Assert.That(result.Name, Is.EqualTo(author.Name));
        }

        [Test]
        public async Task GetAllAuthors()
        {
            var author = new Author { Name = "Author1", Language = "en" };
            await AuthorRepo.Insert(author);
            var result = await AuthorRepo.Get();
            Assert.IsNotNull(result);
        }
        [Test]
        public async Task UpdateAuthor()
        {
            var author = new Author { Name = "Author1", Language = "en" };
            await AuthorRepo.Insert(author);
            author.Name = "Author2";
            var result = await AuthorRepo.Update(author);
            Assert.That(result, Is.EqualTo(author));
        }
        [Test]
        public async Task DeleteAuthor()
        {
            var author = new Author { Name = "Author1", Language = "en" };
            await AuthorRepo.Insert(author);
            var result = await AuthorRepo.Delete(author.Id);
            Assert.That(result, Is.EqualTo(author));
        }

        [Test]
        public async Task GetAuthorByIdThrowsEntityNotFoundException()
        {
            Assert.ThrowsAsync<EntityNotFoundException>(async () => await AuthorRepo.GetOneById(0));
        }
        [Test]
        public async Task GetAuthorByNameThrowsEntityNotFoundException()
        {
            Assert.ThrowsAsync<EntityNotFoundException>(async () => await ((AuthorRepository)AuthorRepo).GetAuthorByName(""));
        }
        [Test]
        public async Task UpdateAuthorThrowsEntityNotFoundException()
        {
            var author = new Author { Name = "Author1", Language = "en" };
            Assert.ThrowsAsync<DbUpdateConcurrencyException>(async () => await AuthorRepo.Update(author));
        }
        [Test]
        public async Task DeleteAuthorThrowsEntityNotFoundException()
        {
            Assert.ThrowsAsync<EntityNotFoundException>(async () => await AuthorRepo.Delete(0));
        }
    }
}
