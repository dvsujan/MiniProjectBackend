using LibraryManagemetApi.Contexts;
using LibraryManagemetApi.Exceptions;
using LibraryManagemetApi.Interfaces;
using LibraryManagemetApi.Models;
using LibraryManagemetApi.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryTesting
{
    internal class BorrowRepositoryTest
    {
        DbContextOptionsBuilder optionsBuilder;
        IRepository<int, Payment> paymentRepo;
        IRepository<int, Card> cardRepository;
        IRepository<int, User> userRepository;
        IRepository<int, Borrowed> BorrowedRepository;
        IRepository<int, Author> AuthorRepo;
        IRepository<int, Book> BookRepo;
        IRepository<int, Publisher> publisherRepo;
        IRepository<int, Category> categoryRepo;
        IRepository<int, Location> locationRepo;
        IRepository<int, Role> roleRepo;

        Author author;
        Publisher publisher;
        Category category;
        Location location;
        User user;
        Card card;
        Role role;
        Book book;

        int authorId;
        int publisherId;
        int categoryId;
        int locationId;
        int userId;
        int cardId;
        int roleId;
        int bookId;

        [SetUp]
        public async Task Setup()
        {
            optionsBuilder = new DbContextOptionsBuilder().UseInMemoryDatabase("libraryTest");
            var context = new LibraryManagementContext(optionsBuilder.Options);
            paymentRepo = new PaymentRepository(context);
            cardRepository = new CardRepository(context);
            userRepository = new UserRepository(context);
            BorrowedRepository = new BorrowedRepository(context);
            AuthorRepo = new AuthorRepository(context);
            BookRepo = new BookRepository(context);
            publisherRepo = new PublisherRepository(context);
            categoryRepo = new CategoryRepository(context);
            locationRepo = new LocationRepository(context);
            roleRepo = new RoleRepository(context);

            author = new Author { Name = "Author1", Language = "en" };
            publisher = new Publisher { Name = "Publisher1", Address = "5th Ave , NY" };
            category = new Category { Name = "Literacture" };
            location = new Location { Floor = 1, Shelf = 1 };
            role = new Role { Name = "Admin" };

            author = await AuthorRepo.Insert(author);
            publisher = await publisherRepo.Insert(publisher);
            category = await categoryRepo.Insert(category);
            location = await locationRepo.Insert(location);
            role = await roleRepo.Insert(role);

            authorId = author.Id;
            publisherId = publisher.Id;
            categoryId = category.Id;
            locationId = location.Id;
            roleId = role.Id;

            user = new User
            {
                Username = "ramu",
                Email = "ramu@gmail.com",
                Password = new byte[] { 1, 2, 3, 4 },
                HashKey = new byte[] { 1, 2, 3, 4 },
                Active = true,
                RoleId = roleId
            };
            book = new Book
            {
                Title = "Book1",
                AuthorId = authorId,
                ISBN = "123456789",
                PublishedDate = DateTime.Now,
                PublisherId = publisherId,
                CategoryId = categoryId,
                LocationId = locationId
            };
            user = await userRepository.Insert(user);
            book = await BookRepo.Insert(book);
            userId = user.Id;
            bookId = book.Id;
        }

        [Test]
        public async Task AddBorrowed()
        {
            var context = new LibraryManagementContext(optionsBuilder.Options);
            var borrowed = new Borrowed
            {
                UserId = userId,
                BookId = bookId,
                BorrowedDate = DateTime.Now,
                DueDate = DateTime.Now.AddDays(7)
            };
            await BorrowedRepository.Insert(borrowed);
            var result = BorrowedRepository.GetOneById(borrowed.Id);
            Assert.IsNotNull(result);
        }
        [Test]
        public async Task UpdateBorrowed()
        {
            var context = new LibraryManagementContext(optionsBuilder.Options);
            var borrowed = new Borrowed
            {
                UserId = userId,
                BookId = bookId,
                BorrowedDate = DateTime.Now,
                DueDate = DateTime.Now.AddDays(7)
            };
            await BorrowedRepository.Insert(borrowed);
            borrowed.DueDate = DateTime.Now.AddDays(14);
            await BorrowedRepository.Update(borrowed);
            var result = await BorrowedRepository.GetOneById(borrowed.Id);
            Assert.AreEqual(result.DueDate.Date, DateTime.Now.Date.AddDays(14));
        }

        [Test]
        public async Task DeleteBorrowed()
        {
            var context = new LibraryManagementContext(optionsBuilder.Options);
            var borrowed = new Borrowed
            {
                UserId = userId,
                BookId = bookId,
                BorrowedDate = DateTime.Now,
                DueDate = DateTime.Now.AddDays(7)
            };
            var inserted = await BorrowedRepository.Insert(borrowed);
            var del = await BorrowedRepository.Delete(inserted.Id);
            Assert.That(del, Is.EqualTo(inserted));
        }
        [Test]
        public async Task DeleteBorrowedExcetion()
        {
            var context = new LibraryManagementContext(optionsBuilder.Options);
            var borrowed = new Borrowed
            {
                UserId = userId,
                BookId = bookId,
                BorrowedDate = DateTime.Now,
                DueDate = DateTime.Now.AddDays(7)
            };
            await BorrowedRepository.Insert(borrowed);
            await BorrowedRepository.Delete(borrowed.Id);
            Assert.ThrowsAsync<EntityNotFoundException>(() => BorrowedRepository.GetOneById(borrowed.Id));
        }
        [Test]
        public async Task GetBorrowedById()
        {
            var context = new LibraryManagementContext(optionsBuilder.Options);
            var borrowed = new Borrowed
            {
                UserId = userId,
                BookId = bookId,
                BorrowedDate = DateTime.Now,
                DueDate = DateTime.Now.AddDays(7)
            };
            await BorrowedRepository.Insert(borrowed);
            var result = await BorrowedRepository.GetOneById(borrowed.Id);
            Assert.IsNotNull(result);
        }
        
        [Test]
        public async Task GetAllBorrowedByDate()
        {
            var context = new LibraryManagementContext(optionsBuilder.Options);
            var borrowed = new Borrowed
            {
                UserId = userId,
                BookId = bookId,
                BorrowedDate = DateTime.Now,
                DueDate = DateTime.Now.AddDays(7)
            };
            await BorrowedRepository.Insert(borrowed);
            var result = await ((BorrowedRepository)BorrowedRepository).GetAllFilgerByDate(DateTime.Now.Date, DateTime.Now.AddDays(7));
            Assert.That(result.Count(), Is.EqualTo(2));
        }

    }
}
