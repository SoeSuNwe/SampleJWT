using Microsoft.AspNetCore.Identity;
using SampleJWT.Dtos;
using SampleJWT.Models;

namespace SampleJWT.Services
{
    public class UserService : IUserService
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IConfiguration _configuration;

        public UserService(
            UserManager<User> userManager,
            SignInManager<User> signInManager,
            IConfiguration configuration)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration;
        }

        public async Task<IdentityResult> RegisterUser(RegisterViewModel model)
        {
            var user = new User { UserName = model.UserName, Email = model.Email };

            // Note: You would typically hash the password before storing it
            var result = await _userManager.CreateAsync(user, model.Password);

            return result;
        }

        public async Task<TokenResult> GenerateJwtToken(LoginViewModel model)
        {
            var user = await _userManager.FindByNameAsync(model.UserName);

            if (user != null && await _userManager.CheckPasswordAsync(user, model.Password))
            {
                var token = JwtTokenGenerator.GenerateJwtToken(user.Id,user.UserName);

                return new TokenResult
                {
                    Success = true,
                    Token = token
                };
            }

            return new TokenResult
            {
                Success = false,
                Errors = new List<string> { "Invalid username or password" }
            };
        }

        public async Task<IdentityResult> DeleteUser(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return IdentityResult.Failed(new IdentityError { Description = "User not found" });
            }

            var result = await _userManager.DeleteAsync(user);

            return result;
        }

        public async Task<IdentityResult> UpdateUser(string userId, UpdateUserViewModel model)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return IdentityResult.Failed(new IdentityError { Description = "User not found" });
            }

            // Update user properties based on the model
            user.Email = model.Email;
            // Update other properties...

            // Note: You would typically validate and update user properties as needed

            var result = await _userManager.UpdateAsync(user);

            return result;
        }
    }
}
