using System.ComponentModel.DataAnnotations;

namespace LibraryManagemetApi.Models
{
    public class Review
    {
        [Key]
        public int Id { get; set; }
        public int UserId { get; set; }
        public int BookId { get; set; }
        public float Rating { get; set; }

        public User User { get; set; }
        public Book Book { get; set; }
    }
}
