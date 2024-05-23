using LibraryManagemetApi.Contexts;
using LibraryManagemetApi.Models;

namespace LibraryManagemetApi.Repositories
{
    public class CardRepository: AbstractRepositoryClass<int, Card>
    {
        private readonly LibraryManagementContext _context;
        public CardRepository(LibraryManagementContext context) : base(context)
        {

        }
    }
}
