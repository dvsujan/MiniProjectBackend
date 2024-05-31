using LibraryManagemetApi.Contexts;
using LibraryManagemetApi.Models;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagemetApi.Repositories
{
    public class ReviewRepository:AbstractRepositoryClass<int, Review>
    {
        private readonly LibraryManagementContext _context;
        public ReviewRepository(LibraryManagementContext context) : base(context)
        {
        }
        public async Task<float> GetAvgRatingOfBookId(int bookId)
        {
            var avgRating = await _dbSet.Where(r => r.BookId == bookId).GroupBy(r => r.BookId).Select(g => g.Average(r => r.Rating)).FirstOrDefaultAsync();
            return avgRating;
        }
        public async Task<int> GetNoOfRatingsOfBookId(int bookId)
        {
            var noOfRatings = await _dbSet.Where(r => r.BookId == bookId).CountAsync();
            return noOfRatings;
        }
    }
}
