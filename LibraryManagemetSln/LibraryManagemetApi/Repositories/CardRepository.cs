using LibraryManagemetApi.Contexts;
using LibraryManagemetApi.Models;

namespace LibraryManagemetApi.Repositories
{
    public class CardRepository: AbstractRepositoryClass<int, Card>
    {
        public CardRepository(LibraryManagementContext context) : base(context)
        {

        }
    }
}
