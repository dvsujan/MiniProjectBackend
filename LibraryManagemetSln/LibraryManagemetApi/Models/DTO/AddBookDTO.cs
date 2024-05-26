using System.ComponentModel.DataAnnotations;

namespace LibraryManagemetApi.Models.DTO
{
    public class AddBookDTO
    {
        [Required]
        [MinLength(3)]
        public string Title { get; set; }= string.Empty;
        [Required]
        [MinLength(3)]
        public string AuthorName { get; set; }= string.Empty;
        [Required]
        [MinLength(3)]
        public string publisherName { get; set; }= string.Empty;
        [Required]
        [MinLength(3)]
        public string CategoryName { get; set; }= "General";
        
        public int LocationId { get; set; } = 1; 
        
        [Required]
        public string ISBN { get; set; }= "0000000000000";
        public int Stock { get; set; } = 1; 
        public DateTime PublishedDate { get; set; }
    }
}