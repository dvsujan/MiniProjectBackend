using System.ComponentModel.DataAnnotations;

namespace LibraryManagemetApi.Models
{
    public class Author
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Language { get; set; } = "English";
    }
}
