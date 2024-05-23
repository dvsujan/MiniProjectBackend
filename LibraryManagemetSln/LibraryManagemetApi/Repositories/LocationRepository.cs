using LibraryManagemetApi.Contexts;
using LibraryManagemetApi.Models;

namespace LibraryManagemetApi.Repositories
{
    public class LocationRepository:AbstractRepositoryClass<int, Location>
    {
        private readonly LibraryManagementContext _context;
        public LocationRepository(LibraryManagementContext context) : base(context)
        {

        }
    }
}
