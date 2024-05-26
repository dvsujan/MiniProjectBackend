using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using System.ComponentModel.DataAnnotations;

namespace LibraryManagemetApi.Models.DTO
{
    public class ActivateUserDTO
    {
        [Required]
        public string Email { get; set; }
        
        [Required]
        public bool IsActive { get; set; }
    }
}