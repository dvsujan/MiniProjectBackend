using LibraryManagemetApi.Interfaces;
using LibraryManagemetApi.Models;
using LibraryManagemetApi.Models.DTO;
using LibraryManagemetApi.Repositories;

namespace LibraryManagemetApi.Services
{
    public class AnaylticsService : IAnalyticsService
    {
        private readonly IRepository<int, Borrowed> _borrowedRepository;
        public AnaylticsService(IRepository<int, Borrowed> borrowedRepository)
        {
            _borrowedRepository = borrowedRepository;
        }

        /// <summary>
        /// return the analytics of the library based on the time frame
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ReturnAnalyticsDTO>> GetAnalytics(AnalyticsDTO dto)
        {
            try
            {
                var borrowed = await ((BorrowedRepository)_borrowedRepository).GetAllFilgerByDate(dto.StartDate, dto.EndDate);
                var booksBorrowed = borrowed.Count();
                var booksReturned = borrowed.Where(x => x.ReturnDate != null).Count();
                return new List<ReturnAnalyticsDTO>
                {
                    new ReturnAnalyticsDTO
                    {
                        BooksBorrowed = booksBorrowed,
                        BooksReturned = booksReturned
                    }
                };
            }
            catch
            {
                throw;
            }

        }

        /// <summary>
        /// returns the overdue books in the library based on the time frame given by the Admin
        /// </summary>
        /// <param name="dto"></param>
        /// <returns>list of no of analytics</returns>

        public async Task<IEnumerable<ReturnODAnalyticsDTO>> returnODAnalyticsDTOs(AnalyticsDTO dto)
        {
            try
            {
                var borrowed = await ((BorrowedRepository)_borrowedRepository).GetAllFilgerByDate(dto.StartDate, dto.EndDate);
                var booksBorrowed = borrowed.Count();
                var booksReturned = borrowed.Where(x => x.ReturnDate != null).Count();
                var booksOverdue = borrowed.Where(x => x.ReturnDate == null && x.DueDate < DateTime.Now).Count();
                return new List<ReturnODAnalyticsDTO>
                {
                    new ReturnODAnalyticsDTO
                    {
                        BooksOverdue = booksOverdue
                    }
                };
            }
            catch
            {
                throw;
            }
        }
    }
}
