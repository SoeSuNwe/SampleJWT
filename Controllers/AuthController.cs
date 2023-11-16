using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using SampleJWT.Models;

namespace SampleJWT.Controllers
{

    public class AuthController : Controller
    {
        private readonly AppDbContext _dbContext;
        public AuthController(AppDbContext context)
        {
            _dbContext = context;
        }



        private string _secretKey = "your_secret_key_here"; // Replace with your secret key

        [HttpPost("/register")]
        public void Register([FromBody] User user)
        {
            _dbContext.Users.Add(user);
            _dbContext.SaveChanges();
        }

        [HttpPost("/login")]
        public string Login([FromBody] User user)
        {
            var dbUser = _dbContext.Users
              .FirstOrDefault(u => u.Username == user.Username && u.Password == user.Password);

            if (dbUser != null)
            {
                // Generate and return JWT token
                return GenerateJwtToken(dbUser.Id, dbUser.Username);
            }

            return "Invalid credentials";
        } 

        private string GenerateJwtToken(int userId, string username)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_secretKey);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                new Claim(ClaimTypes.Name, username),
                new Claim(ClaimTypes.NameIdentifier, userId.ToString())
            }),
                Expires = DateTime.UtcNow.AddHours(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
