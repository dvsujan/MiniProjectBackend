using LibraryManagemetApi.Models.DTO;

namespace LibraryManagemetApi.Interfaces
{
    public interface IUserService
    {
        Task<RegisterReturnDTO> Register(userRegisterDTO user);
        Task<LoginReturnDTO> Login(UserLoginDTO user);
        Task<ActivateReturnDTO> ActivateUser(ActivateUserDTO email);
    }
}
