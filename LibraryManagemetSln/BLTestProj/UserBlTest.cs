using LibraryManagemetApi.Contexts;
using LibraryManagemetApi.Interfaces;
using LibraryManagemetApi.Models;
using LibraryManagemetApi.Models.DTO;
using LibraryManagemetApi.Repositories;
using LibraryManagemetApi.Exceptions;
using LibraryManagemetApi.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System.IdentityModel.Tokens.Jwt;

namespace BLTestProj
{
    public class Tests
    {
        private readonly UserRepository _userRepository;
        private readonly UserService _userService;
        private readonly TokenService _tokenService;

        public Tests()
        {
            LibraryManagementContext _context;
            var options = new DbContextOptionsBuilder<LibraryManagementContext>()
                .UseInMemoryDatabase(databaseName: "LibraryManagement")
                .Options;
            _context = new LibraryManagementContext(options);
            _userRepository = new UserRepository(_context);
            IConfiguration configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
            _tokenService = new TokenService(configuration);
            _userService = new UserService(_userRepository, _tokenService);
        }

        [Test]
        public async Task TestRegister()
        {
            userRegisterDTO user = new userRegisterDTO()
            {
                UserName = "ramuu",
                Email = "ramuu@gmail.com",
                Password = "password"
            };
            RegisterReturnDTO res = await _userService.Register(user);
            Assert.AreEqual(res.Email, user.Email);
        }

        [Test]
        public async Task TestLoginThrowsUserAlreadyExistException()
        {
            userRegisterDTO user = new userRegisterDTO()
            {
                UserName = "ramu",
                Email = "ramu@gmail.com",
                Password = "password"
            };
            //RegisterReturnDTO res = await _userService.Register(user);
            userRegisterDTO userLogin = new userRegisterDTO()
            {
                Email = user.Email,
                Password = user.Password,
                UserName = user.UserName
            };
            Assert.ThrowsAsync<UserAlreadyExistsException>(() => _userService.Register(user));


        }

        [Test]
        public async Task TestLogin()
        {
            userRegisterDTO user = new userRegisterDTO()
            {
                UserName = "som",
                Email = "somu@gmail.com",
                Password = "password"
            };
            RegisterReturnDTO res = await _userService.Register(user);
            UserLoginDTO userLogin = new UserLoginDTO()
            {
                Email = user.Email,
                Password = user.Password
            };
            ActivateUserDTO activateUser = new ActivateUserDTO()
            {
                Email = user.Email
            };
            await _userService.ActivateUser(activateUser);
            LoginReturnDTO loginRes = await _userService.Login(userLogin);
            Assert.AreEqual(loginRes.email, user.Email);
        }
        [Test]
        public async Task TestLoginThrowsIncorrectPasswordException()
        {
            userRegisterDTO user = new userRegisterDTO()
            {
                UserName = "ramu",
                Email = "ramu@gmail.com",
                Password = "password"
            };
            //RegisterReturnDTO res = await _userService.Register(user);
            UserLoginDTO userLogin = new UserLoginDTO()
            {
                Email = user.Email,
                Password = "password1"
            };
            ActivateUserDTO activateUser = new ActivateUserDTO()
            {
                Email = user.Email
            };
            await _userService.ActivateUser(activateUser);
            Assert.ThrowsAsync<IncorrectPasswordExcpetion>(() => _userService.Login(userLogin));
        }
        [Test]
        public async Task TestLoginEntityNotFoundException()
        {
            UserLoginDTO userLoginn = new UserLoginDTO()
            {
                Email = "",
                Password = ""
            };
            Assert.ThrowsAsync<EntityNotFoundException>(() => _userService.Login(userLoginn));

        }
        [Test]
        public async Task TestLoginThrowsUserNotActiveException()
        {
            userRegisterDTO userrs = new userRegisterDTO()
            {
                UserName = "ramua",
                Email = "ramua@gmail.com",
                Password = "password"
            };
            RegisterReturnDTO res = await _userService.Register(userrs);
            UserLoginDTO userLogin = new UserLoginDTO()
            {
                Email = userrs.Email,
                Password = userrs.Password
            };
            Assert.ThrowsAsync<UserNotActiveException>(() => _userService.Login(userLogin));
        }

        [Test]
        public async Task ActivateTestThrowsEntityNotFoundException()
        {
            ActivateUserDTO activateUser = new ActivateUserDTO()
            {
                Email = ""
            }; 
            Assert.ThrowsAsync<EntityNotFoundException>(() => _userService.ActivateUser(activateUser));

        }
    }

}