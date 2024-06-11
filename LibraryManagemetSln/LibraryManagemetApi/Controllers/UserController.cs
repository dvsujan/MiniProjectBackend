using LibraryManagemetApi.Exceptions;
using LibraryManagemetApi.Interfaces;
using LibraryManagemetApi.Models.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManagemetApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly ILogger<UserController> _logger;
        public UserController(IUserService userService,ILogger<UserController> logger)
        {
            _userService = userService;
            _logger = logger;
        }
        
        /// <summary>
        /// login the user and return the token
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("login")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorDTO),StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorDTO),StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorDTO),StatusCodes.Status409Conflict)]
        public async Task<ActionResult<LoginReturnDTO>> Login(UserLoginDTO user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ErrorDTO
                {
                    Code="400",
                    Message="Invalid Data"
                });
            }
            try
            {
                var login = await _userService.Login(user);
                _logger.LogInformation($"User {user.Email} logged in");
                return Ok(login);
            }
            catch (EntityNotFoundException)
            {
                _logger.LogWarning($"User {user.Email} not found");
                return NotFound(new ErrorDTO
                {
                    Code="404",
                    Message="User not found"
                });
            }
            catch (IncorrectPasswordExcpetion)
            {
                _logger.LogWarning($"Incorrect password for user {user.Email}");
                return Conflict(new ErrorDTO
                {
                    Code="409",
                    Message="Incorrect Password"
                });
            }
            catch (UserNotActiveException)
            {
                _logger.LogWarning($"User {user.Email} not active");
                return BadRequest(new ErrorDTO
                {
                    Code="400",
                    Message="User not active"
                });
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        /// <summary>
        /// Register a new user
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("register")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorDTO),StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorDTO),StatusCodes.Status404NotFound)]
        public async Task<ActionResult<RegisterReturnDTO>> Register(userRegisterDTO user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ErrorDTO
                {
                    Code="400",
                    Message="Invalid Data"
                });
            }
            try
            {
                var register = await _userService.Register(user);
                _logger.LogWarning($"User {user.Email} registered");
                return Ok(register);
            }
            catch (UserAlreadyExistsException)
            {
                _logger.LogWarning($"User {user.Email} already exists");
                return BadRequest(new ErrorDTO
                {
                    Code="400",
                    Message="User already exists"
                });
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        /// <summary>
        /// Set the user status as active
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("activate")]
        [Authorize(Roles ="2")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorDTO),StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorDTO),StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ActivateReturnDTO>> ActivateUser(ActivateUserDTO email)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ErrorDTO
                {
                    Code="400",
                    Message="Invalid Data"
                });
            }
            try
            {
                var activate = await _userService.ActivateUser(email);
                _logger.LogInformation($"User {email.Email} activated");
                return Ok(activate);
            }
            catch (EntityNotFoundException)
            {
                _logger.LogWarning($"User {email.Email} not found");
                return NotFound(new ErrorDTO
                {
                    Code="404",
                    Message="User not found"
                });
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}
