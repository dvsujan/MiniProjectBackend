using System.ComponentModel.DataAnnotations;

namespace LibraryManagemetApi.Models
{
    public class Book
    {
        [Key]
        public int Id { get; set; }
        public string Title { get; set; }
        public int AuthorId { get; set; }
        public string ISBN { get; set; }
        public DateTime PublishedDate { get; set; }
        public int PublisherId { get; set; }
        public int CategoryId { get; set; }
        public int LocationId { get; set; }
        public DateTime CreatedAt { get; set; } = System.DateTime.Now;
        public Author Author { get; set; }
        public Publisher Publisher { get; set; }
        public Category Category { get; set; }
        public Location Location { get; set; }
    }
}
