using System.ComponentModel.DataAnnotations;

namespace LibraryManagemetApi.Models.DTO
{
    public class AddCardDTO
    {
        public int UserId { get; set; }
        
        [MinLength(14)]
        [MaxLength(16)]
        public string CardNumber { get; set; }

        public DateTime ExpiryDate { get; set; } = DateTime.Now;
        public int CVV { get; set; }
    }
}