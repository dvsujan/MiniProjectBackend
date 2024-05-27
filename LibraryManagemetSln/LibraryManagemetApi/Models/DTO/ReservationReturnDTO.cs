namespace LibraryManagemetApi.Models.DTO
{
    public class ReservationReturnDTO
    {
        public int ReservationId { get; set; }
        public int UserId { get; set; }
        public int BookId { get; set; }
        public DateTime ReservationDate { get; set; }
    }
}