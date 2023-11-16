using Microsoft.AspNetCore.Identity;
using SampleJWT.Dtos;

namespace SampleJWT.Services
{
    public interface IUserService
    {
        Task<IdentityResult> RegisterUser(RegisterViewModel model);
        Task<TokenResult> GenerateJwtToken(LoginViewModel model);
        Task<IdentityResult> DeleteUser(string userId);
        Task<IdentityResult> UpdateUser(string userId, UpdateUserViewModel model);
    }
}
