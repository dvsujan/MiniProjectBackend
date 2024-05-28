using System.ComponentModel.DataAnnotations;

namespace LibraryManagemetApi.Models
{
    public class Card
    {
        [Key]
        public int Id { get; set; }
        public int UserId { get; set; }
        public string CardNumber { get; set; }
        public DateTime ExpDate { get; set; }
        public int CVV { get; set; }

        public User User { get; set; }
    }
}
