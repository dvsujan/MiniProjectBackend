using LibraryManagemetApi.Contexts;
using LibraryManagemetApi.Models;

namespace LibraryManagemetApi.Repositories
{
    public class PaymentRepository:AbstractRepositoryClass<int, Payment>
    {
        private readonly LibraryManagementContext _context;
        public PaymentRepository(LibraryManagementContext context) : base(context)
        {

        }
    }
}
