using LibraryManagemetApi.Contexts;
using LibraryManagemetApi.Models;

namespace LibraryManagemetApi.Repositories
{
    public class ReviewRepository:AbstractRepositoryClass<int, Review>
    {
        private readonly LibraryManagementContext _context;
        public ReviewRepository(LibraryManagementContext context) : base(context)
        {

        }
    }
}
