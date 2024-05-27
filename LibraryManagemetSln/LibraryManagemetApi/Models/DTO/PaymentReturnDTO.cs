namespace LibraryManagemetApi.Models.DTO
{
    public class PaymentReturnDTO
    {
        public int PaymentId { get; set; }
        public decimal Amount { get; set; }
        public DateTime PaymentDate { get; set; }
    }
}