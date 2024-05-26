namespace LibraryManagemetApi.Models.DTO
{
    public class LoginReturnDTO
    {
        public int id { get; set;  }
        public string email { get; set; } = string.Empty;
        public string token { get; set; } = string.Empty;
    }
}