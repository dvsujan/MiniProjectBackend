using LibraryManagemetApi.Models;

namespace LibraryManagemetApi.Interfaces
{
    public interface ITokenService
    {
        public string GenerateToken(User user);
    }
}
