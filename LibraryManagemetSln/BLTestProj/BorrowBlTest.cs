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
    public class BorrowBlTest
    {
        IRepository<int, Borrowed> _borrowRepository;
        IRepository<int, Stock> _stockRepository;
        IRepository<int, User> _userRepository;
        IRepository<int, Book> _bookRepository;
        IRepository<int, Reservation> _reservationRepository;
        IRepository<int, Payment> _paymentRepository;

        IborrowService _borrowService;

        [SetUp]
        public async Task Setup()
        {

        }

        [Test]
        public async Task TestAddBorrow()
        {
            LibraryManagementContext _context;
            var options = new DbContextOptionsBuilder<LibraryManagementContext>()
                .UseInMemoryDatabase(databaseName: "LibraryManagement")
                .Options;
            _context = new LibraryManagementContext(options);
            _context.Database.EnsureCreated();
            await _context.SaveChangesAsync();

            _context = new LibraryManagementContext(options);
            _borrowRepository = new BorrowedRepository(_context);
            _stockRepository = new StockRepository(_context);
            _userRepository = new UserRepository(_context);
            _bookRepository = new BookRepository(_context);
            _reservationRepository = new ReservationRepository(_context);
            _paymentRepository = new PaymentRepository(_context);
            _borrowService = new BorrowService(_borrowRepository, _stockRepository, _userRepository, _bookRepository, _reservationRepository, _paymentRepository);
            BorrowDTO borrow = new BorrowDTO()
            {
                UserId = 101,
                BookId = 2,
                BorrowDate = DateTime.Now,
            };
            BorrowReturnDTO res = await _borrowService.BorrowBook(borrow);
            Assert.AreEqual(res.UserId, borrow.UserId);
            var delBorrow = await _borrowRepository.GetOneById(res.BorrowId);
            await _borrowRepository.Delete(delBorrow.Id);
        }
        [Test]
        public async Task BorrowBookThrowsBookOutOfStockException()
        {
            LibraryManagementContext _context;
            var options = new DbContextOptionsBuilder<LibraryManagementContext>()
                .UseInMemoryDatabase(databaseName: "LibraryManagement")
                .Options;
            _context = new LibraryManagementContext(options);
            _context.Database.EnsureCreated();
            await _context.SaveChangesAsync();

            _context = new LibraryManagementContext(options);
            _borrowRepository = new BorrowedRepository(_context);
            _stockRepository = new StockRepository(_context);
            _userRepository = new UserRepository(_context);
            _bookRepository = new BookRepository(_context);
            _reservationRepository = new ReservationRepository(_context);
            _paymentRepository = new PaymentRepository(_context);
            _borrowService = new BorrowService(_borrowRepository, _stockRepository, _userRepository, _bookRepository, _reservationRepository, _paymentRepository);

            BorrowDTO borrowe = new BorrowDTO()
            {
                UserId = 1,
                BookId = 4,
                BorrowDate = DateTime.Now,
            };
            Assert.ThrowsAsync<BookOutOfStockException>(async () => await _borrowService.BorrowBook(borrowe));
        }

        [Test]
        public async Task GetBorrowedBookTest() {
            LibraryManagementContext _context;
            var options = new DbContextOptionsBuilder<LibraryManagementContext>()
                .UseInMemoryDatabase(databaseName: "LibraryManagement")
                .Options;
            _context = new LibraryManagementContext(options);
            _context.Database.EnsureCreated();
            await _context.SaveChangesAsync();

            _context = new LibraryManagementContext(options);
            _borrowRepository = new BorrowedRepository(_context);
            _stockRepository = new StockRepository(_context);
            _userRepository = new UserRepository(_context);
            _bookRepository = new BookRepository(_context);
            _reservationRepository = new ReservationRepository(_context);
            _paymentRepository = new PaymentRepository(_context);
            _borrowService = new BorrowService(_borrowRepository, _stockRepository, _userRepository, _bookRepository, _reservationRepository, _paymentRepository);

            var returnDTOs = await _borrowService.GetBorrowedBooks(2);
            Assert.IsNotNull(returnDTOs);
        }

        [Test]
        public async Task GetDueBooksUser() {
            LibraryManagementContext _context;
            var options = new DbContextOptionsBuilder<LibraryManagementContext>()
                .UseInMemoryDatabase(databaseName: "LibraryManagement")
                .Options;
            _context = new LibraryManagementContext(options);
            _context.Database.EnsureCreated();
            await _context.SaveChangesAsync();

            _context = new LibraryManagementContext(options);
            _borrowRepository = new BorrowedRepository(_context);
            _stockRepository = new StockRepository(_context);
            _userRepository = new UserRepository(_context);
            _bookRepository = new BookRepository(_context);
            _reservationRepository = new ReservationRepository(_context);
            _paymentRepository = new PaymentRepository(_context);
            _borrowService = new BorrowService(_borrowRepository, _stockRepository, _userRepository, _bookRepository, _reservationRepository, _paymentRepository);

            var returnDTOs = await _borrowService.GetDueBookeByUser(2);
            Assert.IsNotNull(returnDTOs);
        }
        [Test]
        public async Task ReturnBookTest()
        {
            LibraryManagementContext _context;
            var options = new DbContextOptionsBuilder<LibraryManagementContext>()
                .UseInMemoryDatabase(databaseName: "LibraryManagement")
                .Options;
            _context = new LibraryManagementContext(options);
            _context.Database.EnsureCreated();
            await _context.SaveChangesAsync();

            _context = new LibraryManagementContext(options);
            _borrowRepository = new BorrowedRepository(_context);
            _stockRepository = new StockRepository(_context);
            _userRepository = new UserRepository(_context);
            _bookRepository = new BookRepository(_context);
            _reservationRepository = new ReservationRepository(_context);
            _paymentRepository = new PaymentRepository(_context);
            _borrowService = new BorrowService(_borrowRepository, _stockRepository, _userRepository, _bookRepository, _reservationRepository, _paymentRepository);
            ReturnDTO d = new ReturnDTO()
            {
                UserId = 101,
                BookId = 2
            };
            var res2 = await _borrowService.ReturnBook(d);
            Assert.IsNotNull(res2);
        }
        
        [Test]
        public async Task ReturnBookThrowsBookNotBorrowedException()
        {
            LibraryManagementContext _context;
            var options = new DbContextOptionsBuilder<LibraryManagementContext>()
                .UseInMemoryDatabase(databaseName: "LibraryManagement")
                .Options;
            _context = new LibraryManagementContext(options);
            _context.Database.EnsureCreated();
            await _context.SaveChangesAsync();

            _context = new LibraryManagementContext(options);
            _borrowRepository = new BorrowedRepository(_context);
            _stockRepository = new StockRepository(_context);
            _userRepository = new UserRepository(_context);
            _bookRepository = new BookRepository(_context);
            _reservationRepository = new ReservationRepository(_context);
            _paymentRepository = new PaymentRepository(_context);
            _borrowService = new BorrowService(_borrowRepository, _stockRepository, _userRepository, _bookRepository, _reservationRepository, _paymentRepository);

            ReturnDTO d = new ReturnDTO()
            {
                UserId = 101,
                BookId = 5
            };
            Assert.ThrowsAsync<BookNotBorrowedException>(() => _borrowService.ReturnBook(d));
        }

        [Test]
        public async Task RenewBookTest()
        {
            LibraryManagementContext _context;
            var options = new DbContextOptionsBuilder<LibraryManagementContext>()
                .UseInMemoryDatabase(databaseName: "LibraryManagement")
                .Options;
            _context = new LibraryManagementContext(options);
            _context.Database.EnsureCreated();
            await _context.SaveChangesAsync();

            _context = new LibraryManagementContext(options);
            _borrowRepository = new BorrowedRepository(_context);
            _stockRepository = new StockRepository(_context);
            _userRepository = new UserRepository(_context);
            _bookRepository = new BookRepository(_context);
            _reservationRepository = new ReservationRepository(_context);
            _paymentRepository = new PaymentRepository(_context);
            _borrowService = new BorrowService(_borrowRepository, _stockRepository, _userRepository, _bookRepository, _reservationRepository, _paymentRepository);

            Assert.ThrowsAsync<BookOverDueException>(async () => await _borrowService.renewBook(2, 1));
        }

        [Test]
        public async Task BorrowReservcedBookTest()
        {
            LibraryManagementContext _context;
            var options = new DbContextOptionsBuilder<LibraryManagementContext>()
                .UseInMemoryDatabase(databaseName: "LibraryManagement")
                .Options;
            _context = new LibraryManagementContext(options);
            _context.Database.EnsureCreated();
            await _context.SaveChangesAsync();

            _context = new LibraryManagementContext(options);
            _borrowRepository = new BorrowedRepository(_context);
            _stockRepository = new StockRepository(_context);
            _userRepository = new UserRepository(_context);
            _bookRepository = new BookRepository(_context);
            _reservationRepository = new ReservationRepository(_context);
            _paymentRepository = new PaymentRepository(_context);
            _borrowService = new BorrowService(_borrowRepository, _stockRepository, _userRepository, _bookRepository, _reservationRepository, _paymentRepository);
            // first reserve a book
            ReservationDTO reservation = new ReservationDTO()
            {
                UserId = 101,
                BookId = 3,
                ReservationDate = DateTime.Now,
            };
            BorrowReturnDTO rdto = await _borrowService.BorrowReservedBook(101, 4); 
            Assert.IsNotNull(rdto);

        }

        [Test]
        public async Task RenewBookTestwithoutException()
        {
            LibraryManagementContext _context;
            var options = new DbContextOptionsBuilder<LibraryManagementContext>()
                .UseInMemoryDatabase(databaseName: "LibraryManagement")
                .Options;
            _context = new LibraryManagementContext(options);
            _context.Database.EnsureCreated();
            await _context.SaveChangesAsync();

            _context = new LibraryManagementContext(options);
            _borrowRepository = new BorrowedRepository(_context);
            _stockRepository = new StockRepository(_context);
            _userRepository = new UserRepository(_context);
            _bookRepository = new BookRepository(_context);
            _reservationRepository = new ReservationRepository(_context);
            _paymentRepository = new PaymentRepository(_context);
            _borrowService = new BorrowService(_borrowRepository, _stockRepository, _userRepository, _bookRepository, _reservationRepository, _paymentRepository);
            Assert.ThrowsAsync<BookOverDueException>(async () => await _borrowService.renewBook(2, 1));
        }

    }
}
