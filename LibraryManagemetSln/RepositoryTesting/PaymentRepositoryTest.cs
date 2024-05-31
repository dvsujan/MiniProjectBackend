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

namespace RepositoryTesting
{
    public class PaymentRepositoryTest
    {
         DbContextOptionsBuilder optionsBuilder;
        IRepository<int, Payment> paymentRepo;
        IRepository<int, Card> cardRepository;
        IRepository<int, User> userRepository;
        IRepository<int, Borrowed> BorrowedRepository ;
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
        Borrowed borrowed;
        Role role;
        Book book; 

        int authorId;
        int publisherId;
        int categoryId;
        int locationId;
        int userId;
        int cardId;
        int BorrowedId;
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
            card = new Card
            {
                CardNumber = "123456789",
                ExpDate = DateTime.Now,
                CVV = 123,
                UserId = userId
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
            card = await cardRepository.Insert(card);
            book = await BookRepo.Insert(book);
            userId = user.Id;
            cardId = card.Id;
            bookId = book.Id;
            borrowed = new Borrowed
            {
                UserId = userId,
                BookId = bookId,
                BorrowedDate = DateTime.Now,
                DueDate = DateTime.Now.AddDays(7)
            };
            borrowed = await BorrowedRepository.Insert(borrowed);
            BorrowedId = borrowed.Id;
        }

        [Test]
        public async Task AddPayment()
        {
            var payment = new Payment
            {
                CardId = cardId,
                BorrowedId = BorrowedId,
                Amount = 100,
                PaymentDate = DateTime.Now
            };
            var result = await paymentRepo.Insert(payment);
            Assert.AreEqual(payment, result);
        }
        [Test]
        public async Task GetPaymentById()
        {
            var payment = new Payment
            {
                CardId = cardId,
                BorrowedId = BorrowedId,
                Amount = 100,
                PaymentDate = DateTime.Now
            };
            var result = await paymentRepo.Insert(payment);
            var paymentById = await paymentRepo.GetOneById(payment.Id);
            Assert.IsNotNull(paymentById);
        }
        [Test]
        public async Task UpdatePayment()
        {
            var payment = new Payment
            {
                CardId = cardId,
                BorrowedId = BorrowedId,
                Amount = 100,
                PaymentDate = DateTime.Now
            };
            var result = await paymentRepo.Insert(payment);
            result.Amount = 200;
            var updatedPayment = await paymentRepo.Update(result);
            Assert.AreEqual(updatedPayment.Amount, 200);
        }
        [Test]
        public async Task DeletePayment()
        {
            var payment = new Payment
            {
                CardId = cardId,
                BorrowedId = BorrowedId,
                Amount = 100,
                PaymentDate = DateTime.Now
            };
            var result = await paymentRepo.Insert(payment);
            var deletedPayment = await paymentRepo.Delete(result.Id);
            Assert.ThrowsAsync<EntityNotFoundException>(async () => await paymentRepo.GetOneById(deletedPayment.Id));
        }
        [Test]
        public async Task DeletePaymentThrowsEntityNotFoundException()
        {
            Assert.ThrowsAsync<EntityNotFoundException>(async () => await paymentRepo.Delete(0));
        }
        [Test]
        public async Task GetPayments()
        {
            var payment = new Payment
            {
                CardId = cardId,
                BorrowedId = BorrowedId,
                Amount = 100,
                PaymentDate = DateTime.Now
            };
            var result = await paymentRepo.Insert(payment);
            var payments = await paymentRepo.Get();
            Assert.IsNotNull(payments);
        }
    }
}
