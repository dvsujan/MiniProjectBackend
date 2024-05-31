using LibraryManagemetApi.Contexts;
using LibraryManagemetApi.Models;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagemetApi.Repositories
{
    public class BorrowedRepository: AbstractRepositoryClass<int, Borrowed>
    {
        private readonly LibraryManagementContext _context;
        public BorrowedRepository(LibraryManagementContext context) : base(context)
        {

        }
        /// <summary>
        /// returns the list of borrowed books by the given timespan 
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="enddate"></param>
        /// <returns></returns>
        public async Task<IEnumerable<Borrowed>> GetAllFilgerByDate(DateTime startDate , DateTime enddate) {
            var borrowed = await _dbSet.Where(x => x.BorrowedDate >= startDate && x.BorrowedDate <= enddate).ToListAsync();
            return borrowed;
        }
    }
}
