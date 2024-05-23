using LibraryManagemetApi.Contexts;
using LibraryManagemetApi.Models;

namespace LibraryManagemetApi.Repositories
{
    public class StockRepository:AbstractRepositoryClass<int, Stock>
    {
        private readonly LibraryManagementContext _context;
        public StockRepository(LibraryManagementContext context) : base(context)
        {

        }
    }
}
