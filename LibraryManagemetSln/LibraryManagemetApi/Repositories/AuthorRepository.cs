using LibraryManagemetApi.Contexts;
using LibraryManagemetApi.Models;

namespace LibraryManagemetApi.Repositories
{
    public class AuthorRepository: AbstractRepositoryClass<int, Author>
    {
        private readonly LibraryManagementContext _context;
        public AuthorRepository(LibraryManagementContext context) : base(context)
        {
        }
    }
}
