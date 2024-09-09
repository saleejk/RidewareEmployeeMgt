using Microsoft.AspNetCore.Mvc;
using RidewareEmployeeMgt.Interfaces;
using RidewareEmployeeMgt.Models;

namespace RidewareEmployeeMgt.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        public readonly IUserServices _userServices;
        private readonly ILogger<UserController> logger;
        public UserController(IUserServices userServices, ILogger<UserController> logger)
        {
            _userServices = userServices;
            this.logger = logger;
        }

        [HttpPost("register")]
        public async Task<IActionResult> RegisterUser(UserDto userDto)
        {
            try
            {
                var response = await _userServices.RegisterUser(userDto);
                if (response != null)
                {
                    return Ok(response);
                }
                logger.LogInformation("user already exist");
                return Conflict("employee already exist");
            }
            catch (Exception ex)
            {
                return StatusCode(500, "an error occured while registering the user");
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> LogIn(UserDto userDto)
        {
            try
            {
                var token = await _userServices.LogIn(userDto);

                if (token != null)
                {
                    return Ok(token);
                }

                return Unauthorized("Invalid email or password");
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An internal server error occurred.");
            }
        }


    }
}
