namespace LibraryManagemetApi.Models.DTO
{
    public class AddBookDTO
    {
        public string Title { get; set; } 
        public int AuthorId { get; set; }   
        public int PublisherId { get; set; }
        public int CategoryId { get; set; }
        public int LocationId { get; set; }
        public string ISBN { get; set; }
        public DateTime PublishedDate { get; set; }
    }
}