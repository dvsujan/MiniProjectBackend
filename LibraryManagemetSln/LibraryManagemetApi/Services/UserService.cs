using LibraryManagemetApi.Exceptions;
using LibraryManagemetApi.Interfaces;
using LibraryManagemetApi.Models;
using LibraryManagemetApi.Models.DTO;
using LibraryManagemetApi.Repositories;
using System.Security.Cryptography;
using System.Text;

namespace LibraryManagemetApi.Services
{
    public class UserService : IUserService
    {
        private readonly IRepository<int, User> _userRepository;
        private readonly ITokenService _tokenService;
        public UserService(IRepository<int, User> userRepository, ITokenService tokenService)
        {
            _userRepository = userRepository;
            _tokenService = tokenService;
        }
        public async Task<LoginReturnDTO> Login(UserLoginDTO user)
        {
            try
            {
                var userDB = await ((UserRepository)_userRepository).GetUserByEmail(user.Email);
                HMACSHA512 hMACSHA = new HMACSHA512(userDB.HashKey);
                var encrypterPass = hMACSHA.ComputeHash(Encoding.UTF8.GetBytes(user.Password));
                bool isPasswordSame = ComparePassword(encrypterPass, userDB.Password);
                if (isPasswordSame)
                {
                    if (!userDB.Active)
                    {
                        throw new UserNotActiveException();
                    }
                    var token = _tokenService.GenerateToken(userDB);
                    return new LoginReturnDTO
                    {
                        id = userDB.Id,
                        email = userDB.Email,
                        token = token
                    };
                }
                else
                {
                    throw new IncorrectPasswordExcpetion();
                }
            }
            catch (EntityNotFoundException)
            {
                throw new EntityNotFoundException();
            }
            catch (IncorrectPasswordExcpetion)
            {
                throw new IncorrectPasswordExcpetion();
            }
            catch (UserNotActiveException)
            {
                throw new UserNotActiveException();
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
        private bool ComparePassword(byte[] encrypterPass, byte[] password)
        {
            for (int i = 0; i < encrypterPass.Length; i++)
            {
                if (encrypterPass[i] != password[i])
                {
                    return false;
                }
            }
            return true;
        }

        public async Task<RegisterReturnDTO> Register(userRegisterDTO user)
        {
            User userReg = null;
            try
            {
                userReg = new User
                {
                    Email = user.Email,
                    Username = user.UserName,
                    RoleId = 1,
                    Active = true
                };
                HMACSHA512 hMACSHA = new HMACSHA512();
                userReg.HashKey = hMACSHA.Key;
                userReg.Password = hMACSHA.ComputeHash(Encoding.UTF8.GetBytes(user.Password));
                await _userRepository.Insert(userReg);
                return new RegisterReturnDTO
                {
                    Id = userReg.Id,
                    Email = userReg.Email,
                    UserName = userReg.Username
                };
            }
            catch (EntityNotFoundException)
            {
                throw new EntityNotFoundException();
            }
            catch (Exception e)
            {
                if (userReg != null)
                {
                    await ReverseUserCreation(userReg.Id);
                }
                throw;
            }
        }
        private async Task ReverseUserCreation(int id)
        {
            await _userRepository.Delete(id);
        }

        public async Task<ActivateReturnDTO> ActivateUser(ActivateUserDTO activateuserDTO)
        {
            try
            {
                var user = await ((UserRepository)_userRepository).GetUserByEmail(activateuserDTO.Email);
                user.Active = true;
                await _userRepository.Update(user);
                return new ActivateReturnDTO
                {
                    Email = user.Email,
                    Active = user.Active
                };
            }
            catch (EntityNotFoundException)
            {
                throw new EntityNotFoundException();
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
    }
}
