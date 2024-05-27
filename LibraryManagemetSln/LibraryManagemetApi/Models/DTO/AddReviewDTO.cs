using System.ComponentModel.DataAnnotations;

namespace LibraryManagemetApi.Models.DTO
{
    public class AddReviewDTO
    {
        public int UserId { get; set;  }
        public int BookId { get; set; }

        [Range(1, 5)]
        public float Rating { get; set; }

        [MinLength(3)]
        public string Comment { get; set; }
    }
}