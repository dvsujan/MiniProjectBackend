using LibraryManagemetApi.Contexts;
using LibraryManagemetApi.Models;

namespace LibraryManagemetApi.Repositories
{
    public class BookRepository : AbstractRepositoryClass<int, Book>
    {
        private readonly LibraryManagementContext _context;
        public BookRepository(LibraryManagementContext context) : base(context)
        {
        }
    }
}
