using System.ComponentModel.DataAnnotations;

namespace LibraryManagemetApi.Models
{
    public class Reservation
    {
        [Key]
        public int Id { get; set; }
        public int UserId { get; set; }
        public int BookId { get; set; }
        public DateTime ReservationDate { get; set; } = System.DateTime.Now;

        public User User { get; set; }
        public Book Book { get; set; }
    }
}
