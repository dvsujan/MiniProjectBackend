using LibraryManagemetApi.Contexts;
using LibraryManagemetApi.Exceptions;
using LibraryManagemetApi.Models;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagemetApi.Repositories
{
    public class StockRepository:AbstractRepositoryClass<int, Stock>
    {
        private readonly LibraryManagementContext _context;
        public StockRepository(LibraryManagementContext context) : base(context)
        {
        
        }
        /// <summary>
        /// gets the stock by book Id
        /// </summary>
        /// <param name="bookId"></param>
        /// <returns></returns>
        /// <exception cref="EntityNotFoundException"></exception>
        public async Task<Stock> GetStockByBookId(int bookId)
        {
            var stock = await _dbSet.FirstOrDefaultAsync(s => s.BookId == bookId);
            if (stock == null)
            {
                throw new EntityNotFoundException();
            }
            return stock;
        }
    }
}
