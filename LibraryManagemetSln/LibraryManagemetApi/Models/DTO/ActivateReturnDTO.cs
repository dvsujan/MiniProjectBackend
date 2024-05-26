namespace LibraryManagemetApi.Models.DTO
{
    public class ActivateReturnDTO
    {
        public int UserId { get; set; }
        public string Email { get; set; }
        public bool Active { get; set; }
    }
}