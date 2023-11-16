using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SampleJWT.Dtos;
using SampleJWT.Services;

namespace SampleJWT.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterViewModel model)
        {
            // Validate the model
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Attempt to register the user
            IdentityResult result = await _userService.RegisterUser(model);

            if (result.Succeeded)
            {
                return Ok(new { Message = "User registered successfully" });
            }

            return BadRequest(new { Message = "Registration failed", Errors = result.Errors });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginViewModel model)
        {
            // Validate the model
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Attempt to authenticate the user and generate a JWT token
            var tokenResult = await _userService.GenerateJwtToken(model);

            if (tokenResult.Success)
            {
                return Ok(new { Token = tokenResult.Token, Message = "Login successful" });
            }

            return Unauthorized(new { Message = "Authentication failed", Errors = tokenResult.Errors });
        }

        [Authorize]
        [HttpDelete("delete")]
        public async Task<IActionResult> Delete()
        {
            // Get the user id from the claims
            var userId = User.FindFirst("sub")?.Value;

            // Attempt to delete the user
            var result = await _userService.DeleteUser(userId);

            if (result.Succeeded)
            {
                return Ok(new { Message = "User deleted successfully" });
            }

            return BadRequest(new { Message = "Deletion failed", Errors = result.Errors });
        }

        [Authorize]
        [HttpPut("update")]
        public async Task<IActionResult> Update([FromBody] UpdateUserViewModel model)
        {
            // Validate the model
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Get the user id from the claims
            var userId = User.FindFirst("sub")?.Value;

            // Attempt to update the user
            var result = await _userService.UpdateUser(userId, model);

            if (result.Succeeded)
            {
                return Ok(new { Message = "User updated successfully" });
            }

            return BadRequest(new { Message = "Update failed", Errors = result.Errors });
        }
    }
}
