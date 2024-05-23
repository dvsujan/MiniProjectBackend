using LibraryManagemetApi.Contexts;
using LibraryManagemetApi.Models;

namespace LibraryManagemetApi.Repositories
{
    public class PublisherRepository:AbstractRepositoryClass<int, Publisher>
    {
        private readonly LibraryManagementContext _context;
        public PublisherRepository(LibraryManagementContext context) : base(context)
        {

        }
    }
}
