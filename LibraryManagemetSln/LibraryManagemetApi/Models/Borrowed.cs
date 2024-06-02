using System.ComponentModel.DataAnnotations;

namespace LibraryManagemetApi.Models
{
    public class Borrowed
    {
        [Key]
        public int Id { get; set; }
        public int UserId { get; set; }
        public int BookId { get; set; }
        public DateTime BorrowedDate { get; set; } = System.DateTime.Now;
        public DateTime DueDate { get; set; } = System.DateTime.Now.AddDays(7);
        public DateTime? ReturnDate { get; set; }

        public User User { get; set; }
        public Book Book { get; set; }
    }
}
