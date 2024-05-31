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
        public UserController(IUserService userService)
        {
            _userService = userService;
        }
        
        [HttpPost]
        [Route("login")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<ActionResult<LoginReturnDTO>> Login(UserLoginDTO user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            try
            {
                var login = await _userService.Login(user);
                return Ok(login);
            }
            catch (EntityNotFoundException)
            {
                return NotFound(user);
            }
            catch (IncorrectPasswordExcpetion)
            {
                return Conflict(user);
            }
            catch (UserNotActiveException)
            {
                return BadRequest(user);
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
        
        [HttpPost]
        [Route("register")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<RegisterReturnDTO>> Register(userRegisterDTO user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            try
            {
                var register = await _userService.Register(user);
                return Ok(register);
            }
            catch (UserAlreadyExistsException)
            {
                return BadRequest();
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
        [HttpPost]
        [Route("activate")]
        [Authorize(Roles ="2")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ActivateReturnDTO>> ActivateUser(ActivateUserDTO email)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            try
            {
                var activate = await _userService.ActivateUser(email);
                return Ok(activate);
            }
            catch (EntityNotFoundException)
            {
                return NotFound(email);
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}
