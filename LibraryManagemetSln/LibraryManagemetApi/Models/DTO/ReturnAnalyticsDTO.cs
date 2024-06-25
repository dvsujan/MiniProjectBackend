namespace LibraryManagemetApi.Models.DTO
{
    public class ReturnAnalyticsDTO
    {
        public int BooksBorrowed { get; set; }
        public int BooksReturned { get; set; }
        public DateTime Date { get; set; }
    }
}