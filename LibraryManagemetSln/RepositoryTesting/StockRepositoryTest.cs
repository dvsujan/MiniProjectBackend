using LibraryManagemetApi.Contexts;
using LibraryManagemetApi.Exceptions;
using LibraryManagemetApi.Interfaces;
using LibraryManagemetApi.Models;
using LibraryManagemetApi.Repositories;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryTesting
{
    internal class StockRepositoryTest
    {
        DbContextOptionsBuilder optionBuilder; 
        IRepository<int, Stock> stockReository;
        IRepository<int, Author> AuthorRepo;
        IRepository<int, Book> BookRepo;
        IRepository<int, Publisher> publisherRepo;
        IRepository<int, Category> categoryRepo;
        IRepository<int, Location> locationRepo;
        IRepository<int, Book> bookRepo;
        Author author;
        Publisher publisher;
        Category category;
        Location location;
        Book book; 
        int authorId;
        int publisherId;
        int categoryId;
        int locationId;
        int bookId; 


        [SetUp]
        public async Task Setup()
        {
            optionBuilder = new DbContextOptionsBuilder().UseInMemoryDatabase("libraryTest");
            var context = new LibraryManagementContext(optionBuilder.Options);
            stockReository = new StockRepository(context);
            AuthorRepo = new AuthorRepository(context);
            BookRepo = new BookRepository(context);
            publisherRepo = new PublisherRepository(context);
            categoryRepo = new CategoryRepository(context);
            locationRepo = new LocationRepository(context);
            bookRepo = new BookRepository(context);
            author = new Author { Name = "Author1", Language = "en" };
            publisher = new Publisher { Name = "Publisher1", Address = "5th Ave , NY" };
            category = new Category { Name = "Literacture" };
            location = new Location { Floor = 1, Shelf = 1 };
            await AuthorRepo.Insert(author);
            await publisherRepo.Insert(publisher);
            await categoryRepo.Insert(category);
            await locationRepo.Insert(location);
            book = new Book
            {
                Title = "Book1",
                AuthorId = author.Id,
                ISBN = "123456789",
                PublisherId = publisher.Id,
                CategoryId = category.Id,
                LocationId = location.Id
            };
            authorId = author.Id;
            publisherId = publisher.Id;
            categoryId = category.Id;
            locationId = location.Id;
            bookId = book.Id;
        }

        [Test]
        public async Task AddStock()
        {
            Stock stock = new Stock { Quantity = 10, BookId = bookId};
            await stockReository.Insert(stock);
            var result = await stockReository.GetOneById(1);
            Assert.AreEqual(stock, result);
        }
        [Test]
        public async Task UpdateStock()
        {
            Stock stock = new Stock { Quantity = 10, BookId = bookId };
            await stockReository.Insert(stock);
            stock.Quantity = 20;
            var res = await stockReository.Update(stock);
            var result = await stockReository.GetOneById(res.Id);
            Assert.AreEqual(stock.Id, result.Id);
        }

        [Test]
        public async Task DeleteStock()
        {
            Stock stock = new Stock { Quantity = 10, BookId = bookId };
            var res = await stockReository.Insert(stock);
            var result = await stockReository.Delete(res.Id);
            Assert.IsNotNull(result);
        }

        [Test]
        public async Task DeleteException()
        {
            Assert.ThrowsAsync<EntityNotFoundException>(async () => await stockReository.Delete(4));
        }
        
        [Test]
        public async Task GetAllStock()
        {
            Stock stock = new Stock { Quantity = 10, BookId = bookId };
            await stockReository.Insert(stock);
            var result = await stockReository.Get();
            Assert.AreEqual(2, result.Count());
        }
        
        [Test]
        public async Task GetOneById()
        {
            Stock stock = new Stock { Quantity = 10, BookId = bookId };
            var res = await stockReository.Insert(stock);
            var result = await stockReository.GetOneById(res.Id);
            Assert.AreEqual(stock.Id, result.Id);
        }
        [Test]
        public async Task GetStockByBookId()
        {
            Stock stock = new Stock { Quantity = 10, BookId = bookId };
            await stockReository.Insert(stock);
            var result = await ((StockRepository)stockReository).GetStockByBookId(bookId);
            Assert.IsNotNull(result); 
        }
        [Test]
        public async Task GetStockByBookIdException()
        {
            Assert.ThrowsAsync<EntityNotFoundException>(async () => await ((StockRepository)stockReository).GetStockByBookId(4));
            
        }
    }
}
