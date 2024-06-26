namespace LibraryManagemetApi.Models.DTO
{
    public class ReturnUserReviewDTO
    {
        public int ReviewId { get; set; }
        public int UserId { get; set; }
        public int BookId { get; set; }
        public float Rating { get; set; }
        public string Comment { get; set; } = "";
        public string BookTitle { get; set; } = "";
    }
}
