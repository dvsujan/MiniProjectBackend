using LibraryManagemetApi.Contexts;
using LibraryManagemetApi.Interfaces;
using LibraryManagemetApi.Models;
using LibraryManagemetApi.Exceptions;
using LibraryManagemetApi.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace RepositoryTestProj
{
    public class Tests
    {
        DbContextOptionsBuilder optionsBuilder;
        IRepository<int, Author> AuthorRepo;
        IRepository<int, Book> BookRepo;
        IRepository<int, Publisher> publisherRepo;
        IRepository<int, Category> categoryRepo;
        IRepository<int, Location> locationRepo;
        Author author;
        Publisher publisher;
        Category category;
        Location location;
        int authorId;
        int publisherId;
        int categoryId;
        int locationId;

        [SetUp]
        public async Task Setup()
        {
            optionsBuilder = new DbContextOptionsBuilder().UseInMemoryDatabase("libraryTest");
            var context = new LibraryManagementContext(optionsBuilder.Options);
            AuthorRepo = new AuthorRepository(context);
            BookRepo = new BookRepository(context);
            publisherRepo = new PublisherRepository(context);
            categoryRepo = new CategoryRepository(context);
            locationRepo = new LocationRepository(context);
            author = new Author { Name = "Author1", Language = "en" };
            publisher = new Publisher { Name = "Publisher1", Address = "5th Ave , NY" };
            category = new Category { Name = "Literacture" };
            location = new Location { Floor = 1, Shelf = 1 };
            await AuthorRepo.Insert(author);
            await publisherRepo.Insert(publisher);
            await categoryRepo.Insert(category);
            await locationRepo.Insert(location);
            authorId = author.Id;
            publisherId = publisher.Id;
            categoryId = category.Id;
            locationId = location.Id;
        }

        [Test]
        public async Task AddBook()
        {
            var context = new LibraryManagementContext(optionsBuilder.Options);
            var book = new Book
            {
                Title = "Book1",
                AuthorId = authorId,
                ISBN = "123456789",
                PublishedDate = DateTime.Now,
                PublisherId = publisherId,
                CategoryId = categoryId,
                LocationId = locationId
            };
            await BookRepo.Insert(book);
            var result = BookRepo.GetOneById(book.Id);
            Assert.IsNotNull(result);
        }
        [Test]
        public async Task UpdateBook()
        {
            var context = new LibraryManagementContext(optionsBuilder.Options);
            var book = new Book
            {
                Title = "Book1",
                AuthorId = authorId,
                ISBN = "123456789",
                PublishedDate = DateTime.Now,
                PublisherId = publisherId,
                CategoryId = categoryId,
                LocationId = locationId
            };
            await BookRepo.Insert(book);
            book.Title = "Book2";
            await BookRepo.Update(book);
            var result = await BookRepo.GetOneById(book.Id);
            Assert.AreEqual(result.Title, "Book2");
        }
        [Test]
        public async Task DeleteBook()
        {
            var context = new LibraryManagementContext(optionsBuilder.Options);
            var book = new Book
            {
                Title = "Book1",
                AuthorId = authorId,
                ISBN = "123456789",
                PublishedDate = DateTime.Now,
                PublisherId = publisherId,
                CategoryId = categoryId,
                LocationId = locationId
            };
            await BookRepo.Insert(book);
            await BookRepo.Delete(book.Id);
            Assert.ThrowsAsync<EntityNotFoundException>(() => BookRepo.GetOneById(book.Id));
        }
        
        [Test]
        public async Task GetBooks()
        {
            var context = new LibraryManagementContext(optionsBuilder.Options);
            var book = new Book
            {
                Title = "Book1",
                AuthorId = authorId,
                ISBN = "123456789",
                PublishedDate = DateTime.Now,
                PublisherId = publisherId,
                CategoryId = categoryId,
                LocationId = locationId
            };
            await BookRepo.Insert(book);
            var result = await BookRepo.Get();
            Assert.IsNotNull(result);
        }
    }
}