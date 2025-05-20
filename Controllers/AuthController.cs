using Microsoft.AspNetCore.Mvc;
using AccessControlAPI.Models;
using System.Security.Cryptography;
using System.Text;

namespace AccessControlAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly AppDbContext _context;

        public AuthController(AppDbContext context)
        {
            _context = context;
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginRequest request)
        {
            string hashedPassword = ComputeSha256Hash(request.Password);

            var user = _context.Clients.FirstOrDefault(c =>
                c.Username == request.Username &&
                c.PasswordHash == hashedPassword &&
                c.IsActive &&
                (c.ExpiryDate == null || c.ExpiryDate > DateTime.UtcNow)
            );

            if (user == null)
                return Unauthorized(new { message = "ACCESS_DENIED" });

            return Ok(new { message = "ACCESS_GRANTED" });
        }

        private string ComputeSha256Hash(string rawData)
        {
            using (SHA256 sha256Hash = SHA256.Create())
            {
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(rawData));
                StringBuilder builder = new StringBuilder();
                foreach (byte b in bytes)
                    builder.Append(b.ToString("x2"));
                return builder.ToString();
            }
        }
    }
}
