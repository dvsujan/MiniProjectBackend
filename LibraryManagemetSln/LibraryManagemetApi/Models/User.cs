using System.ComponentModel.DataAnnotations;

namespace LibraryManagemetApi.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }
        public string Username { get; set; }
        public byte[] Password { get; set; }
        public byte[] HashKey { get; set; }
        public string Email { get; set; }
        public bool Active { get; set; }
        public int RoleId { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public Role Role { get; set; }
    }

}
