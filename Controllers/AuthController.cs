using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using SampleJWT.Dtos;
using SampleJWT.Models;
using SampleJWT.Services;

namespace SampleJWT.Controllers
{

    public class AuthController : Controller
    {
        private readonly AppDbContext _dbContext;
        public AuthController(AppDbContext context)
        {
            _dbContext = context;
        }

        [HttpPost("/register")]
        public void Register([FromBody] User user)
        {
            _dbContext.Users.Add(user);
            _dbContext.SaveChanges();
        }

        [HttpPost("/login")]
        public string Login([FromBody] LoginViewModel user)
        {
            var dbUser = _dbContext.Users
              .FirstOrDefault(u => u.UserName == user.UserName && u.Password == user.Password);

            if (dbUser != null)
            {
                // Generate and return JWT token
                return JwtTokenGenerator.GenerateJwtToken(dbUser.Id, dbUser.UserName);
            }

            return "Invalid credentials";
        }
    }
}
