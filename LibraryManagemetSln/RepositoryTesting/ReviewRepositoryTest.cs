using LibraryManagemetApi.Contexts;
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

    internal class ReviewRepositoryTest
    {
        DbContextOptionsBuilder optionsBuilder;
        IRepository<int, Card> cardRepository;
        IRepository<int, User> userRepository;
        IRepository<int, Author> AuthorRepo;
        IRepository<int, Book> BookRepo;
        IRepository<int, Publisher> publisherRepo;
        IRepository<int, Category> categoryRepo;
        IRepository<int, Location> locationRepo;
        IRepository<int, Role> roleRepo;
        IRepository<int,Review> reviewRepo;  

        Author author;
        Publisher publisher;
        Category category;
        Location location;
        User user;
        Role role;
        Book book;

        int authorId;
        int publisherId;
        int categoryId;
        int locationId;
        int userId;
        int roleId;
        int bookId;

        [SetUp]
        public async Task Setup()
        {
            optionsBuilder = new DbContextOptionsBuilder().UseInMemoryDatabase("libraryTest");
            var context = new LibraryManagementContext(optionsBuilder.Options);
            cardRepository = new CardRepository(context);
            userRepository = new UserRepository(context);
            AuthorRepo = new AuthorRepository(context);
            BookRepo = new BookRepository(context);
            publisherRepo = new PublisherRepository(context);
            categoryRepo = new CategoryRepository(context);
            locationRepo = new LocationRepository(context);
            roleRepo = new RoleRepository(context);
            reviewRepo = new ReviewRepository(context);

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
        public async Task InsertReview()
        {
            Review review = new Review
            {
                BookId = bookId,
                UserId = userId,
                Rating = 5,
                Comment = "Good Book"
            };
            review = await reviewRepo.Insert(review);
            Assert.IsNotNull(review);
        }
        [Test]
        public async Task GetOneById()
        {
            Review review = new Review
            {
                BookId = bookId,
                UserId = userId,
                Rating = 5,
                Comment = "Good Book"
            };
            review = await reviewRepo.Insert(review);
            var result = reviewRepo.GetOneById(review.Id);
            Assert.IsNotNull(result);
        }
        [Test]
        public async Task GetAll()
        {
            Review review = new Review
            {
                BookId = bookId,
                UserId = userId,
                Rating = 5,
                Comment = "Good Book"
            };
            review = await reviewRepo.Insert(review);
            var result = reviewRepo.Get();
            Assert.IsNotNull(result);
        }
        [Test]
        public async Task UpdateReview()
        {
            Review review = new Review
            {
                BookId = bookId,
                UserId = userId,
                Rating = 5,
                Comment = "Good Book"
            };
            review = await reviewRepo.Insert(review);
            review.Comment = "Very Good Book";
            await reviewRepo.Update(review);
            var result = await reviewRepo.GetOneById(review.Id);
            Assert.AreEqual(result.Comment, "Very Good Book");
        }
        [Test]
        public async Task DeleteReview()
        {
            Review review = new Review
            {
                BookId = bookId,
                UserId = userId,
                Rating = 5,
                Comment = "Good Book"
            };
            review = await reviewRepo.Insert(review);
            var result = await reviewRepo.Delete(review.Id);
            Assert.IsNotNull(result);
        }
        [Test]
        public async Task DeleteReviewException()
        {
            Review review = new Review
            {
                BookId = bookId,
                UserId = userId,
                Rating = 5,
                Comment = "Good Book"
            };
            review = await reviewRepo.Insert(review);
            var result = await reviewRepo.Delete(review.Id);
            Assert.IsNotNull(result);
        }

        [Test]
        public async Task GetAvgRatingOfBook()
        {
            Review review = new Review
            {
                BookId = bookId,
                UserId = userId,
                Rating = 5,
                Comment = "Good Book"
            };
            review = await reviewRepo.Insert(review);
            var result = await ((ReviewRepository)reviewRepo).GetAvgRatingOfBookId(bookId);
            Assert.AreEqual(result, 5);
        }
        
        [Test]
        public async Task GetNoOfReviews()
        {
            Review review = new Review
            {
                BookId = bookId,
                UserId = userId,
                Rating = 5,
                Comment = "Good Book"
            };
            review = await reviewRepo.Insert(review);
            var result = ((ReviewRepository)reviewRepo).GetNoOfRatingsOfBookId(bookId);
            Assert.IsNotNull(result);
        }
    }
}
