using System.ComponentModel.DataAnnotations;

namespace LibraryManagemetApi.Models.DTO
{
    public class EditpublicationDTO
    {
        public int Id { get; set;  }
        [MinLength(3)]
        public string Name { get; set; }
        [MinLength(3)]
        public string Address { get; set; }

    }
}