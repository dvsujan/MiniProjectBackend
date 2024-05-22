using System.ComponentModel.DataAnnotations;

namespace LibraryManagemetApi.Models
{
    public class Payment
    {
        [Key]
        public int Id { get; set; }
        public int UserId { get; set; }
        public int CardId { get; set; }
        public decimal Amount { get; set; }
        public DateTime PaymentDate { get; set; } = System.DateTime.Now;
        public int BorrowedId { get; set; }
        public User User { get; set; }
        public Card Card { get; set; }
        public Borrowed Borrowed { get; set; }
    }
}
