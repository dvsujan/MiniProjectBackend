using LibraryManagemetApi.Contexts;
using LibraryManagemetApi.Interfaces;
using LibraryManagemetApi.Repositories;
using LibraryManagemetApi.Services;
using LibraryManagemetApi.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LibraryManagemetApi.Models.DTO;
using LibraryManagemetApi.Exceptions;

namespace BLTestProj
{
    public class BookBLTest
    {
        IRepository<int, Book> _bookRepository;
        IRepository<int, Author> _authorRepository;
        IRepository<int, Publisher> _publisherRepository;
        IRepository<int, Category> _categoryRepository;
        IRepository<int, Location> _locationRepository;
        IRepository<int, Stock> _stockRepository;
        IRepository<int, Review> _reviewRepository;
        IBookService _bookService;

        [SetUp]
        public async Task Setup()
        {
            LibraryManagementContext _context;
            var options = new DbContextOptionsBuilder<LibraryManagementContext>()
                .UseInMemoryDatabase(databaseName: "LibraryManagement")
                .Options;
            _context = new LibraryManagementContext(options);
            _context.Database.EnsureCreated();
            await _context.SaveChangesAsync();

            _context = new LibraryManagementContext(options);
            await _context.Database.EnsureCreatedAsync(); 
            _bookRepository = new BookRepository(_context);
            _authorRepository = new AuthorRepository(_context);
            _publisherRepository = new PublisherRepository(_context);
            _categoryRepository = new CategoryRepository(_context);
            _locationRepository = new LocationRepository(_context);
            _stockRepository = new StockRepository(_context);
            _reviewRepository = new ReviewRepository(_context);
            _bookService = new BookService(_bookRepository, _authorRepository, _publisherRepository, _categoryRepository, _locationRepository, _stockRepository, _reviewRepository);
        }

        [Test]
        public async Task AddBookTest()
        {
            AddBookDTO dto = new AddBookDTO()
            {
                Title = "Book5",
                ISBN = "12345678923",
                LocationId = 1,
                AuthorName = "Author5",
                publisherName = "Publisher5",
                CategoryName = "Category5",
                PublishedDate = DateTime.Now,
            }; 
            var result = await _bookService.AddBook(dto);
            Assert.AreEqual(dto.Title, result.Title);
        }
        [Test]
        public async Task GetAllBooks()
        {
            var result = await _bookService.GetAllBooks(1,10);
            Assert.IsNotNull(result);
        }

        [Test]
        public async Task GetBookById()
        {
            AddBookDTO dto = new AddBookDTO()
            {
                Title = "Book2323",
                ISBN = "1234567892233",
                LocationId = 1,
                AuthorName = "Author5",
                publisherName = "Publisher5",
                CategoryName = "Category5",
                PublishedDate = DateTime.Now,
            };
            var book = await _bookService.AddBook(dto);
            var result = await _bookService.GetBook(book.BookId);
            Assert.IsNotNull(result);
        }
        [Test]
        public async Task SearchBoTitle()
        {
            AddBookDTO dto = new AddBookDTO()
            {
                Title = "Book232323",
                ISBN = "123456789222333",
                LocationId = 1,
                AuthorName = "Author5",
                publisherName = "Publisher5",
                CategoryName = "Category5",
                PublishedDate = DateTime.Now,
            };
            var book = await _bookService.AddBook(dto);
            var result = await _bookService.SearchBookByTitle("Bo");
            Assert.IsNotNull(result);
        }
        [Test]
        public async Task EditBook()
        {
            UpdateBookDTO dto = new UpdateBookDTO()
            {
                BookId=2,
                Title = "Book2",
                ISBN = "12345678923",
                LocationId = 1,
                AuthorName = "Author5",
                publisherName = "Publisher5",
                CategoryName = "Category5",
                PublishedDate = DateTime.Now,
            };
            var result = await _bookService.UpdateBook(dto);
            Assert.AreEqual(dto.Title, result.Title);
        }

        [Test]
        public async Task DeleteBook()
        {
            var result = await _bookService.DeleteBook(1);
            Assert.IsNotNull(result);
        }

        [Test]
        public async Task EditAuthor()
        {

            EditAuthorDTO editdto = new EditAuthorDTO()
            {
                Name = "Author5",
                AuthorLanguage = "spanish",
            };
            var result = await _bookService.EditAuthor(editdto);
            Assert.AreEqual(editdto.Name, result.AuthorName);
        }

        [Test]
        public async Task EditAuthorThrowsException()
        {
            EditAuthorDTO editdto = new EditAuthorDTO()
            {
                Name = "Author52323",
                AuthorLanguage = "spanish",
            };
            Assert.ThrowsAsync<EntityNotFoundException>(async () => await _bookService.EditAuthor(editdto));
        }
        
        [Test]
        public async Task EditPublisher()
        {
            EditpublicationDTO editdto = new EditpublicationDTO()
            {
                Name = "Publisher5",
                Address = "Address5",
            };
            var result = await _bookService.EditPublication(editdto);
            Assert.AreEqual(editdto.Name, result.Name);
        }
    }
}
