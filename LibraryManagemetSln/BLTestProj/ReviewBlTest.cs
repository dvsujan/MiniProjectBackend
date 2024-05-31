using LibraryManagemetApi.Contexts;
using LibraryManagemetApi.Exceptions;
using LibraryManagemetApi.Interfaces;
using LibraryManagemetApi.Models;
using LibraryManagemetApi.Models.DTO;
using LibraryManagemetApi.Repositories;
using LibraryManagemetApi.Services;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLTestProj
{
    internal class ReviewBlTest
    {
        IRepository<int, Review> _reviewRepository;
        ReviewService _reviewService;

        [SetUp]
        public async Task Setup()
        {
            LibraryManagementContext _context;
            var options = new DbContextOptionsBuilder<LibraryManagementContext>()
                .UseInMemoryDatabase(databaseName: "LibraryManagement")
                .Options;
            _context = new LibraryManagementContext(options);
            _context = new LibraryManagementContext(options);
            _reviewRepository = new ReviewRepository(_context); 
            _reviewService = new ReviewService(_reviewRepository);
        }
        
        [Test]
        public async Task TestAddReview()
        {
            AddReviewDTO review = new AddReviewDTO()
            {
                UserId = 1,
                BookId = 1,
                Rating = 5,
                Comment = "Good Book"
            };
            ReturnReviewDTO res = await _reviewService.AddReview(review);
            Assert.AreEqual(res.UserId, review.UserId);
            var delReview = await _reviewRepository.GetOneById(res.ReviewId);
            await _reviewRepository.Delete(delReview.Id);
        }

        [Test]
        public async Task TestAddReviewThrowsReviewAlreadyExistException()
        {
            AddReviewDTO review2 = new AddReviewDTO()
            {
                UserId = 1,
                BookId = 1,
                Rating = 5,
                Comment = "Good Book"
            };
            var res = await _reviewService.AddReview(review2);
            AddReviewDTO review1 = new AddReviewDTO()
            {
                UserId = 1,
                BookId = 1,
                Rating = 5,
                Comment = "Good Book"
            };
            Assert.ThrowsAsync<ReviewAlreadyExistException>(() => _reviewService.AddReview(review1));
            var delReview = await _reviewRepository.GetOneById(res.ReviewId);
            await _reviewRepository.Delete(delReview.Id);
        }

        [Test]
        public async Task TestDeleteReview()
        {
            AddReviewDTO review = new AddReviewDTO()
            {
                UserId = 1,
                BookId = 1,
                Rating = 5,
                Comment = "Good Book"
            };
            ReturnReviewDTO res = await _reviewService.AddReview(review);
            ReturnReviewDTO res1 = await _reviewService.DeleteReview(res.ReviewId, res.UserId);
            Assert.AreEqual(res.ReviewId, res1.ReviewId);
        }

        [Test]
        public async Task TestDeleteReviewThrowsForbiddenUserException()
        {
            AddReviewDTO review = new AddReviewDTO()
            {
                UserId = 1,
                BookId = 1,
                Rating = 5,
                Comment = "Good Book"
            };
            ReturnReviewDTO res = await _reviewService.AddReview(review);
            Assert.ThrowsAsync<ForbiddenUserException>(() => _reviewService.DeleteReview(res.ReviewId, 2));
            var delReview = await _reviewRepository.GetOneById(res.ReviewId);
            await _reviewRepository.Delete(delReview.Id);
        }

        [Test]
        public async Task GetReviewByUserId()
        {
            AddReviewDTO review = new AddReviewDTO()
            {
                UserId = 1,
                BookId = 1,
                Rating = 5,
                Comment = "Good Book"
            };
            ReturnReviewDTO res = await _reviewService.AddReview(review);
            var res1 = await _reviewService.GetReviewByUserId(1);
            Assert.IsNotNull(res1);
            var delReview = await _reviewRepository.GetOneById(res.ReviewId);
            await _reviewRepository.Delete(delReview.Id);
        }

        [Test]
        public async Task GetReviewByBookIdTest()
        {
            AddReviewDTO review = new AddReviewDTO()
            {
                UserId = 1,
                BookId = 1,
                Rating = 5,
                Comment = "Good Book"
            };
            ReturnReviewDTO res = await _reviewService.AddReview(review);
            var res1 = await _reviewService.GetReviwsByBookId(1);
            Assert.IsNotNull(res1);
            var delReview = await _reviewRepository.GetOneById(res.ReviewId);
            await _reviewRepository.Delete(delReview.Id);
        }
    }
}
