using System.ComponentModel.DataAnnotations;

namespace LibraryManagemetApi.Models
{
    public class Publisher
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string? Address { get; set; }

    }
}
