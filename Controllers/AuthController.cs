using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using SampleJWT.Models;

namespace SampleJWT.Controllers
{
   
    public class AuthController : Controller
    {
        private readonly Dictionary<string, string> _users = new Dictionary<string, string>
    {
        {"john_doe", "password123"}
    };

        private string _secretKey = "your_secret_key_here"; // Replace with your secret key

        [HttpPost("/register")]
        public void Register([FromBody] User user)
        {
            // In a real application, you would hash and salt the password before storing it.
            // For simplicity, we are storing the password as plain text in this example.
            _users.Add(user.Username, user.Password);
        }

        [HttpPost("/login")]
        public string Login([FromBody] User user)
        {
            if (ValidateUser(user.Username, user.Password, out var userId))
            {
                // Generate and return JWT token
                return GenerateJwtToken(userId, user.Username);
            }

            return "Invalid credentials";
        }

        private bool ValidateUser(string username, string password, out int userId)
        {
            userId = -1;

            // In a real application, you would retrieve the hashed password from the database.
            if (_users.TryGetValue(username, out var storedPassword) && storedPassword == password)
            {
                // In a real application, you would retrieve the user ID from the database.
                // For simplicity, we are using a constant value here.
                userId = 1;
                return true;
            }

            return false;
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
