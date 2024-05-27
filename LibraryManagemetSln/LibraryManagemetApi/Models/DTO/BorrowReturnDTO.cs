namespace LibraryManagemetApi.Models.DTO
{
    public class BorrowReturnDTO
    {
        public int BorrowId { get; set; }
        public int UserId { get; set; }
        public int BookId { get; set; }
        public DateTime BorrowDate { get; set; }
        public DateTime DueDate { get; set; }
        public decimal Fine { get; set; } = 0; 
    }
}