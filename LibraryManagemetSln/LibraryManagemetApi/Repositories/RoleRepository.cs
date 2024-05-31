using LibraryManagemetApi.Contexts;
using LibraryManagemetApi.Models;
using System.Diagnostics.CodeAnalysis;

namespace LibraryManagemetApi.Repositories
{
    [ExcludeFromCodeCoverage]
    public class RoleRepository:AbstractRepositoryClass<int, Role>
    {
        private readonly LibraryManagementContext _context;
        public RoleRepository(LibraryManagementContext context) : base(context)
        {

        }
    }
}
