namespace LibraryManagemetApi.Models.DTO
{
    public class ReservationDTO
    {
        public int UserId { get; set; }
        public int BookId { get; set; }
        public DateTime ReservationDate { get; set; }= System.DateTime.Now;
    }
}