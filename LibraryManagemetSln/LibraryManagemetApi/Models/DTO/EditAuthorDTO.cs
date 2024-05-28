using System.ComponentModel.DataAnnotations;

namespace LibraryManagemetApi.Models.DTO
{
    public class EditAuthorDTO
    {
        [MinLength(3)]
        public string Name { get; set; } = string.Empty; 
        [MinLength(3)]
        public string AuthorLanguage { get; set; } = string.Empty;
    }
}