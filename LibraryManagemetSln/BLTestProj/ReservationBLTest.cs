using LibraryManagemetApi.Contexts;
using LibraryManagemetApi.Exceptions;
using LibraryManagemetApi.Interfaces;
using LibraryManagemetApi.Models;
using LibraryManagemetApi.Models.DTO;
using LibraryManagemetApi.Repositories;
using LibraryManagemetApi.Services;
using Microsoft.EntityFrameworkCore;

namespace BLTestProj
{
    public class ReservationBLTest
    {
        IRepository<int, Reservation> _reservationRepository;
        IRepository<int, Stock> _stockRepository;
        IRepository<int, Book> _bookRepository;
        IRepository<int, User> _userRepository;
        IRepository<int, Borrowed> _borrowrepository;
        ReservationService _reservationService;
        LibraryManagementContext _context;

        [SetUp]
        public async Task Setup()
        {
            var options = new DbContextOptionsBuilder<LibraryManagementContext>()
                .UseInMemoryDatabase(databaseName: "LibraryManagement")
                .Options;
            _context = new LibraryManagementContext(options);
            await _context.Database.EnsureCreatedAsync(); 
            _context = new LibraryManagementContext(options);
            _reservationRepository = new ReservationRepository(_context);
            _stockRepository = new StockRepository(_context);
            _bookRepository = new BookRepository(_context);
            _userRepository = new UserRepository(_context);
            _borrowrepository = new BorrowedRepository(_context);
            _reservationService = new ReservationService(_reservationRepository, _stockRepository, _userRepository, _bookRepository, _borrowrepository);
        }

        [Test]
        public async Task TestAddReservation()
        {
            ReservationDTO reservation = new ReservationDTO()
            {
                UserId = 101,
                BookId = 3,
                ReservationDate = DateTime.Now,
            };
        }


        [Test]
        public async Task TestAddReservationThrowsBookOutOfStockException()
        {
            ReservationDTO reservation3 = new ReservationDTO()
            {
                UserId = 3,
                BookId = 4,
                ReservationDate = DateTime.Now,
            };
            Assert.ThrowsAsync<BookOutOfStockException>(() => _reservationService.ReserveBook(reservation3));
        }
        [Test]
        public async Task TestCancelReservation()
        {
            var result2 = await _reservationService.CancelReservation(2, 102);
            Assert.IsNotNull(result2);
        }
    }
}
