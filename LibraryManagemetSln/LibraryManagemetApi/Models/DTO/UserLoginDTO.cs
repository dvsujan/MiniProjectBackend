using System.ComponentModel.DataAnnotations;

namespace LibraryManagemetApi.Models.DTO
{
    public class UserLoginDTO
    {
        [EmailAddress]
        public string Email { get; set; }= string.Empty;

        [MinLength(6)]
        public string Password { get; set; } = string.Empty;

    }
}
