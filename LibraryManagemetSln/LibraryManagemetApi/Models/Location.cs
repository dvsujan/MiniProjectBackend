using System.ComponentModel.DataAnnotations;

namespace LibraryManagemetApi.Models
{
    public class Location
    {
        [Key]
        public int Id { get; set; }
        public int Floor { get; set; }
        public int Shelf { get; set; }
    }
}
