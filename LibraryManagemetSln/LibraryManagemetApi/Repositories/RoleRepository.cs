using LibraryManagemetApi.Contexts;
using LibraryManagemetApi.Models;

namespace LibraryManagemetApi.Repositories
{
    public class RoleRepository:AbstractRepositoryClass<int, Role>
    {
        private readonly LibraryManagementContext _context;
        public RoleRepository(LibraryManagementContext context) : base(context)
        {

        }
    }
}
