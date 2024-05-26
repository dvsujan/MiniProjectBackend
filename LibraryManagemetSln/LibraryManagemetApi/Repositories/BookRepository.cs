using LibraryManagemetApi.Contexts;
using LibraryManagemetApi.Models;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagemetApi.Repositories
{
    public class BookRepository : AbstractRepositoryClass<int, Book>
    {
        private readonly LibraryManagementContext _context;
        public BookRepository(LibraryManagementContext context) : base(context)
        {
        }

        internal async Task<IEnumerable<object>> GetBookByTitle(string title)
        {
            var books = await _dbSet.Where(b => b.Title.Contains(title)).ToListAsync();
            return books;
        }
    }
}
