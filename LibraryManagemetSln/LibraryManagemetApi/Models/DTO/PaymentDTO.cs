namespace LibraryManagemetApi.Models.DTO
{
    public class PaymentDTO
    {
        public int UserId { get; set; }
        public int BorrowId { get; set; }
        public int CardId { get; set; }
    }
}