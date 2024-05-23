using LibraryManagemetApi.Contexts;
using LibraryManagemetApi.Models;

namespace LibraryManagemetApi.Repositories
{
    public class ReservationRepository:AbstractRepositoryClass<int, Reservation>
    {
        private readonly LibraryManagementContext _context;
        public ReservationRepository(LibraryManagementContext context) : base(context)
        {

        }
    }
}
