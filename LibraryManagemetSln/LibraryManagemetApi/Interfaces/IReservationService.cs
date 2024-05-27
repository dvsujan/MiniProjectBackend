using LibraryManagemetApi.Models.DTO;

namespace LibraryManagemetApi.Interfaces
{
    public interface IReservationService
    {
        Task<ReservationReturnDTO> ReserveBook(ReservationDTO reservation);
        Task<ReservationReturnDTO> CancelReservation(int reservationId);
    }
}
