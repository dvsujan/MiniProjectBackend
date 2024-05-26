using Microsoft.Identity.Client;
using System.ComponentModel.DataAnnotations;

namespace LibraryManagemetApi.Models.DTO
{
    public class userRegisterDTO
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        public string UserName { get; set; } = string.Empty;

        [MinLength(6)]
        [MaxLength(15)]
        public string Password { get; set; } = string.Empty;
    }
}