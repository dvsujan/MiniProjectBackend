using LibraryManagemetApi.Exceptions;
using LibraryManagemetApi.Interfaces;
using LibraryManagemetApi.Models;
using LibraryManagemetApi.Models.DTO;
using LibraryManagemetApi.Repositories;

namespace LibraryManagemetApi.Services
{
    public class BorrowService:IborrowService
    {
        private readonly IRepository<int, Borrowed> _borrowedRepository;
        private readonly IRepository<int, Stock> _stockRepository;
        private readonly IRepository<int, User> _userRepository;
        private readonly IRepository<int, Book> _bookRepository;
        private readonly IRepository<int , Reservation> _reservationRepository;
        private readonly IRepository<int, Payment> _paymentRepository; 
        
        public BorrowService(IRepository<int, Borrowed> borrowedRepository, IRepository<int, Stock> stockRepository, IRepository<int, User> userRepository, IRepository<int, Book> bookRepository, IRepository<int, Reservation> reservationRepository, IRepository<int, Payment> paymentRepository)
        {
            _borrowedRepository = borrowedRepository;
            _stockRepository = stockRepository;
            _userRepository = userRepository;
            _bookRepository = bookRepository;
            _reservationRepository = reservationRepository;
            _paymentRepository = paymentRepository;
        }

        /// <summary>
        /// used to borrow the book from userId and BookId
        /// </summary>
        /// <param name="borrow"></param>
        ///  <exception cref="BookAlreadyBorrowedException">thorws if book is already Borrows</exception>
        ///  <exception cref="BookOutOfStockException">thorws if book is out of stock</exception>
        ///  <exception cref="BookAlreadyBorrowedException">thorws if book is already Borrows</exception>
        /// <returns>returns the borrow return dto</returns>
        public async Task<BorrowReturnDTO> BorrowBook(BorrowDTO borrow)
        {
            try
            {
                var stock = await ((StockRepository )_stockRepository).GetStockByBookId(borrow.BookId); 
                if (stock.Quantity == 0)
                {
                    throw new BookOutOfStockException();
                }
                var user = await _userRepository.GetOneById(borrow.UserId);
                var book = await _bookRepository.GetOneById(borrow.BookId);
                var BorrowedList = await _borrowedRepository.Get();
                var isBorrowed = BorrowedList.Where(x => x.UserId == borrow.UserId && x.BookId == borrow.BookId && x.ReturnDate == null).FirstOrDefault();
                if (isBorrowed != null)
                {
                    throw new BookAlreadyBorrowedException();
                }
                var reservation = await _reservationRepository.Get();
                foreach (var res in reservation)
                {
                    if (res.UserId == borrow.UserId && res.BookId == borrow.BookId)
                    {
                        throw new BookAlreadyReservedException();
                    }
                }
                stock.Quantity--;
                await _stockRepository.Update(stock);
                Borrowed borrowed = new Borrowed
                {
                    UserId = borrow.UserId,
                    BookId = borrow.BookId,
                    BorrowedDate = System.DateTime.Now,
                    DueDate = System.DateTime.Now.AddDays(7)
                };
                await _borrowedRepository.Insert(borrowed);
                return new BorrowReturnDTO
                {
                    BorrowId = borrowed.Id,
                    UserId = borrowed.UserId,
                    BookId = borrowed.BookId,
                    BorrowDate = borrowed.BorrowedDate,
                    DueDate = borrowed.DueDate,
                };
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// used to borrow reserved book 
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="bookId"></param>
        ///  <exception cref="BookOutOfStockException">thorws if book is out of stock</exception>
        ///  <exception cref="BookNotReservedException">thorws if book is not reserved by the user</exception>
        /// <returns></returns>

        public async Task<BorrowReturnDTO> BorrowReservedBook(int userId, int bookId)
        {
            try
            {
                var stock = await ((StockRepository )_stockRepository).GetStockByBookId(bookId); 
                var user = await _userRepository.GetOneById(userId);
                var book = await _bookRepository.GetOneById(bookId);
                var reservation = await _reservationRepository.Get();
                var allborrows = await _borrowedRepository.Get();
                var borrow = allborrows.Where(x => x.UserId == userId && x.BookId == bookId && x.ReturnDate == null).FirstOrDefault();
                if (borrow != null)
                {
                    throw new BookAlreadyBorrowedException();
                }
                Reservation res = null;
                foreach (var r in reservation)
                {
                    if (r.UserId == userId && r.BookId == bookId)
                    {
                        res = r;
                        break;
                    }
                }
                if (res == null)
                {
                    throw new BookNotReservedException();
                }
                Borrowed borrowed = new Borrowed
                {
                    UserId = userId,
                    BookId = bookId,
                    BorrowedDate = System.DateTime.Now,
                    DueDate = System.DateTime.Now.AddDays(7)
                };
                await _borrowedRepository.Insert(borrowed);
                await _reservationRepository.Delete(res.Id);
                return new BorrowReturnDTO
                {
                    BorrowId = borrowed.Id,
                    UserId = borrowed.UserId,
                    BookId = borrowed.BookId,
                    BorrowDate = borrowed.BorrowedDate,
                    DueDate = borrowed.DueDate,
                };
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// returns all the borrowed books of the user 
        /// </summary>
        /// <param name="UserId"></param>
        /// <returns></returns>
        public async Task<IEnumerable<BorrowReturnDTO>> GetBorrowedBooks(int UserId)
        {
            try
            {
                var borrowed = await _borrowedRepository.Get();
                List<BorrowReturnDTO> borrowReturnDTOs = new List<BorrowReturnDTO>();
                foreach (var borrow in borrowed)
                {
                    if (borrow.UserId == UserId)
                    {
                        var fine = 0;
                        if (borrow.DueDate < DateTime.Now)
                        {
                            fine = (DateTime.Now - borrow.DueDate).Days * 5;
                        }
                        if (borrow.ReturnDate == null)
                        {
                            borrowReturnDTOs.Add(new BorrowReturnDTO
                            {
                                BorrowId = borrow.Id,
                                UserId = borrow.UserId,
                                BookId = borrow.BookId,
                                BorrowDate = borrow.BorrowedDate,
                                DueDate = borrow.DueDate,
                                Fine = fine
                            });
                        }
                    }
                }
                return borrowReturnDTOs;
            }
            catch
            {
                throw;
            }
            
        }

        /// <summary>
        /// returns all the due books of the user
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<IEnumerable<BorrowReturnDTO>> GetDueBookeByUser(int userId)
        {
            try
            {
                var borrowed = await _borrowedRepository.Get();
                List<BorrowReturnDTO> borrowReturnDTOs = new List<BorrowReturnDTO>();
                foreach (var borrow in borrowed)
                {
                    if (borrow.UserId == userId && borrow.DueDate < DateTime.Now)
                    {
                        if (borrow.ReturnDate == null)
                        {
                            borrowReturnDTOs.Add(new BorrowReturnDTO
                            {
                                BorrowId = borrow.Id,
                                UserId = borrow.UserId,
                                BookId = borrow.BookId,
                                BorrowDate = borrow.BorrowedDate,
                                DueDate = borrow.DueDate,
                                Fine = (DateTime.Now - borrow.DueDate).Days * 5
                            });
                        }
                    }
                }
                return borrowReturnDTOs;
            }
            catch
            {
                throw;
            }
        }
        
        /// <summary>
        /// checks if the payment is made for the due book 
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="BorrowId"></param>
        /// <returns></returns>
        private async Task<bool> isPaymentMade(int userId, int BorrowId)
        {
            var payments = await _paymentRepository.Get();
            foreach (var payment in payments)
            {
                if (payment.UserId == userId && payment.BorrowedId == BorrowId)
                {
                    return true;
                }
            }
            return false;
        }
        
        /// <summary>
        /// renews the book
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="BookId"></param>
        /// <returns></returns>
        public async Task<BorrowReturnDTO> renewBook(int userId, int BookId)
        {
            try
            {
                var borrowed = await _borrowedRepository.Get();
                Borrowed borrow = null;
                var orderedborrowed = borrowed.Where(x => x.UserId == userId && x.BookId == BookId && x.ReturnDate==null).OrderByDescending(x => x.BorrowedDate).ToList();
                if (orderedborrowed.Count > 0)
                {
                    borrow = orderedborrowed[0];
                }
                else
                {
                    throw new BookNotBorrowedException();
                }

                if (borrow.ReturnDate != null)
                {
                    throw new BookAlreadyReturnedException();
                }
                
                if (borrow.UserId != userId)
                {
                    throw new UserNotMatchException();
                }
                if (borrow.DueDate < DateTime.Now)
                {
                    if (!await isPaymentMade(userId, borrow.Id))
                    {
                        throw new BookOverDueException();
                    }
                }
                borrow.DueDate = DateTime.Now.AddDays(7);
                await _borrowedRepository.Update(borrow);
                return new BorrowReturnDTO
                {
                    BorrowId = borrow.Id,
                    UserId = borrow.UserId,
                    BookId = borrow.BookId,
                    BorrowDate = borrow.BorrowedDate,
                    DueDate = borrow.DueDate
                };
            }
            catch
            {
                throw;
            }
        }
        /// <summary>
        /// returns the book borrowed by the user
        /// </summary>
        /// <param name="returnDTO"></param>
        /// <returns></returns>        
        public async Task<ReturnReturnDTO> ReturnBook(ReturnDTO returnDTO)
        {
            try
            {
                var user = await _userRepository.GetOneById(returnDTO.UserId);
                var book = await _bookRepository.GetOneById(returnDTO.BookId);

                var borrowed = await _borrowedRepository.Get();
                Borrowed borrow = null;
                foreach (var bor in borrowed)
                {
                    if (bor.UserId == returnDTO.UserId && bor.BookId == returnDTO.BookId && bor.ReturnDate == null)
                    {
                        borrow = bor;
                        break;
                    }
                }
                if (borrow == null)
                {
                    throw new BookNotBorrowedException();
                }
                if (borrow.DueDate < DateTime.Now)
                {
                    if (!await isPaymentMade(returnDTO.UserId, borrow.Id))
                    {
                        throw new BookOverDueException();
                    }
                }
                borrow.ReturnDate = System.DateTime.Now;
                await _borrowedRepository.Update(borrow);
                var stock = await ((StockRepository)_stockRepository).GetStockByBookId(returnDTO.BookId);
                stock.Quantity++;
                await _stockRepository.Update(stock);
                return new ReturnReturnDTO
                {
                    BorrowId = borrow.Id,
                    UserId = borrow.UserId,
                    BookId = borrow.BookId,
                    BorrowDate = borrow.BorrowedDate,
                    DueDate = borrow.DueDate,
                    ReturnDate = DateTime.Now
                };
            }
            catch
            {
                throw;
            }
        }
    }
}
