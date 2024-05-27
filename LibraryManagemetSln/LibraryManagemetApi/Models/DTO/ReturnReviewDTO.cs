namespace LibraryManagemetApi.Models.DTO
{
    public class ReturnReviewDTO
    {
        public int ReviewId { get; set; }
        public int UserId { get; set; }
        public int BookId { get; set; }
        public float Rating { get; set; }
        public string Comment { get; set; }

    }
}