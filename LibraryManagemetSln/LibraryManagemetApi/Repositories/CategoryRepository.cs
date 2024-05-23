using LibraryManagemetApi.Contexts;
using LibraryManagemetApi.Models;

namespace LibraryManagemetApi.Repositories
{
    public class CategoryRepository:AbstractRepositoryClass<int, Category>
    {
        private readonly LibraryManagementContext _context;
        public CategoryRepository(LibraryManagementContext context) : base(context)
        {
        }
    }
}
