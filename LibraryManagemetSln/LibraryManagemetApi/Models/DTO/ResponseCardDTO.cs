namespace LibraryManagemetApi.Models.DTO
{
    public class ResponseCardDTO
    {
        public int CardId { get; set; }
        public int UserId { get; set; }
        public string cardNumber { get; set; }
        public int cvv { get; set; }
        public DateTime expiryDate { get; set; }
    }
}