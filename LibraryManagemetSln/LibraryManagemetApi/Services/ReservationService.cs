using LibraryManagemetApi.Exceptions;
using LibraryManagemetApi.Interfaces;
using LibraryManagemetApi.Models;
using LibraryManagemetApi.Models.DTO;
using LibraryManagemetApi.Repositories;
using Microsoft.AspNetCore.Routing.Constraints;
using System.Threading;

namespace LibraryManagemetApi.Services
{
    public class ReservationService : IReservationService
    {
        private readonly IRepository<int, Reservation> _reservationRepository;
        private readonly IRepository<int, Stock> _stockRepository;
        private readonly IRepository<int, Book> _bookRepository; 
        private readonly IRepository<int, User> _userRepository;
        private readonly IRepository<int, Borrowed> _borrowrepository;
        public ReservationService(IRepository<int, Reservation> reservationRepository, IRepository<int, Stock> stockRepository, IRepository<int, User> userRepository, IRepository<int , Book> bookRepository, IRepository<int , Borrowed> borrowedRepository)
        {
            _reservationRepository = reservationRepository;
            _stockRepository = stockRepository;
            _userRepository = userRepository;
            _bookRepository = bookRepository; 
            _borrowrepository = borrowedRepository;
        }
        
        /// <summary>
        /// cancels the reservation of the book made by the user
        /// </summary>
        /// <param name="reservationId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<ReservationReturnDTO> CancelReservation(int  reservationId, int userId)
        {
            try
            {
                var reservationSave = await _reservationRepository.GetOneById(reservationId);
                if (reservationSave.UserId != userId)
                {
                    throw new ForbiddenUserException();
                }
                await _reservationRepository.Delete(reservationSave.Id);
                var stock = await ((StockRepository)_stockRepository).GetStockByBookId(reservationSave.BookId);
                stock.Quantity++;
                await _stockRepository.Update(stock);
                return new ReservationReturnDTO
                {
                    ReservationId = reservationSave.Id,
                    UserId = reservationSave.UserId,
                    BookId = reservationSave.BookId,
                    ReservationDate = reservationSave.ReservationDate,
                };
            }
            catch
            {
                throw;
            }
        }
        /// <summary>
        /// reservs the book for the user
        /// </summary>
        /// <param name="reservation"></param>
        /// <returns></returns>
        public async Task<ReservationReturnDTO> ReserveBook(ReservationDTO reservation)
        {
            try
            {
                var book = await _bookRepository.GetOneById(reservation.BookId);
                var stock = await ((StockRepository)_stockRepository).GetStockByBookId(book.Id);
                if (stock.Quantity == 0)
                {
                    throw new BookOutOfStockException();
                }
                var reserved = await _reservationRepository.Get(); 
                var reservedBook = reserved.FirstOrDefault(r => r.BookId == reservation.BookId && r.UserId==reservation.UserId);
                if (reservedBook != null)
                {
                    throw new BookAlreadyReservedException();
                }
                var borrowedall = await _borrowrepository.Get();
                borrowedall = borrowedall.Where(b => b.ReturnDate == null); 
                var alreadyborrowed = borrowedall.Where(b => b.UserId == reservation.UserId && b.BookId==reservation.BookId);
                if(alreadyborrowed.Count() > 0) { throw new BookAlreadyBorrowedException();  }
                var user = await _userRepository.GetOneById(reservation.UserId); 
                Reservation reservationSave = new Reservation
                {
                    UserId = reservation.UserId,
                    BookId = reservation.BookId,
                };
                await _reservationRepository.Insert(reservationSave);
                stock.Quantity--;
                await _stockRepository.Update(stock);
                Thread thread = new Thread(async () => await CheckBorrowed(reservationSave.Id,reservationSave.ReservationDate, reservation.UserId, reservation.BookId));
                thread.Start();
                return new ReservationReturnDTO
                {
                    ReservationId = reservationSave.Id,
                    UserId = reservationSave.UserId,
                    BookId = reservationSave.BookId,
                    ReservationDate = reservationSave.ReservationDate,
                };
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// creats a thread to check if the book is borrowed by the reserved time or it delets the reservation and free the book
        /// </summary>
        /// <param name="reservationId"></param>
        /// <param name="reservationDate"></param>
        /// <param name="userId"></param>
        /// <param name="BookId"></param>
        /// <returns></returns>
        public async Task CheckBorrowed(int reservationId, DateTime reservationDate, int userId , int BookId )
        {
            Thread.Sleep(reservationDate.AddDays(1).Subtract(DateTime.Now));
            var reservation = await _reservationRepository.GetOneById(reservationId); 
            var borrowed = await _borrowrepository.Get();
            var borrowedBook = borrowed.FirstOrDefault(b => b.BookId == BookId && b.UserId == userId);
            if (borrowedBook != null)
            {
                await CancelReservation(reservationId, userId);
            }
        }
    }
}
