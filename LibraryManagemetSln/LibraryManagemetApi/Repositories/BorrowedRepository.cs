using LibraryManagemetApi.Contexts;
using LibraryManagemetApi.Models;

namespace LibraryManagemetApi.Repositories
{
    public class BorrowedRepository: AbstractRepositoryClass<int, Borrowed>
    {
        private readonly LibraryManagementContext _context;
        public BorrowedRepository(LibraryManagementContext context) : base(context)
        {

        }
    }
}
