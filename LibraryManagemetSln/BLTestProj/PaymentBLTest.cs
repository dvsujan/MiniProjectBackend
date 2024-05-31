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
    internal class PaymentBLTest
    {
        private IRepository<int, Payment> _paymentRepository;
        private IRepository<int, Card> _cardRepository; 
        private IRepository<int , User> _userRepository;
        private IRepository<int, Borrowed> _borrowRepository;
        private IPaymentService _paymentService; 
        LibraryManagementContext _context;

        [SetUp]
        public async Task Setup()
        {
            var options = new DbContextOptionsBuilder<LibraryManagementContext>()
                .UseInMemoryDatabase(databaseName: "LibraryManagement")
                .Options;
            _context = new LibraryManagementContext(options);
            await _context.Database.EnsureCreatedAsync();
            await _context.SaveChangesAsync();
            _paymentRepository = new PaymentRepository(_context);
            _cardRepository = new CardRepository(_context);
            _userRepository = new UserRepository(_context);
            _borrowRepository = new BorrowedRepository(_context);
            _paymentService = new PaymentService(_paymentRepository, _cardRepository, _userRepository, _borrowRepository);
        }

        [Test]
        public async Task AddCardTest()
        {
            AddCardDTO dto = new AddCardDTO()
            {
                UserId = 1,
                CardNumber = "1234567891234567",
                CVV = 123,
                ExpiryDate = DateTime.Now
            };
            var result = await _paymentService.AddCard(dto);
            Assert.IsNotNull(result);
        }
        [Test]
        public async Task DeleteCardTest()
        {
            AddCardDTO dto = new AddCardDTO()
            {
                UserId = 1,
                CardNumber = "1234567891234567",
                CVV = 123,
                ExpiryDate = DateTime.Now
            };
            var res = await _paymentService.AddCard(dto);
            var result = await _paymentService.DeleteCard(res.CardId, 1);
            Assert.IsNotNull(result); 
        }
        [Test]
        public async Task DeleteCardTest_ThrowsForbiddenUserException()
        {
            AddCardDTO dto = new AddCardDTO()
            {
                UserId = 1,
                CardNumber = "1234567891234567",
                CVV = 123,
                ExpiryDate = DateTime.Now
            };
            var res = await _paymentService.AddCard(dto);
            Assert.ThrowsAsync<ForbiddenUserException>(async () => await _paymentService.DeleteCard(res.CardId, 2));
        }
        
        [Test]
        public async Task GetAllUserCards()
        {
            var result = await _paymentService.GetAllUserCards(1);
            Assert.IsNotNull(result);
        }
        [Test]
        public async Task GetUserPaymentHistory()
        {
            var result = await _paymentService.GetUserPaymentHistory(1);
            Assert.IsNotNull(result);
        }

        [Test]
        public async Task PayFineTest()
        {
            PaymentDTO dto = new PaymentDTO()
            {
                UserId = 101,
                CardId = 2,
                BorrowId = 2,
            }; 
            var result = await _paymentService.PayFine(dto);
            Assert.IsNotNull(result);
        }

        [Test]
        public async Task PayFineTest_ThrowsForbiddenUserException()
        {
            PaymentDTO dto = new PaymentDTO()
            {
                UserId = 101,
                CardId = 1,
                BorrowId = 2,
            };
            Assert.ThrowsAsync<ForbiddenCardException>(async () => await _paymentService.PayFine(dto));
        }
    }
}
