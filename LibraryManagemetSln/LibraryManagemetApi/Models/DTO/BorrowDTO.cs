namespace LibraryManagemetApi.Models.DTO
{
    public class BorrowDTO
    {
        public int UserId { get; set; }
        public int BookId { get; set; }
        public DateTime BorrowDate { get; set; }
    }
}