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
            var user = _context.Clients.FirstOrDefault(c => c.Username == request.Username);
            if (user == null)
                return Ok("INCONNU");

            string enteredHash = ComputeSha256Hash(request.Password);
            if (!user.PasswordHash.Equals(enteredHash, StringComparison.OrdinalIgnoreCase))
                return Ok("MAUVAIS_MDP");

            if (!user.IsActive)
                return Ok("COMPTE_DESACTIVE");

            return Ok("ACCESS_GRANTED");
        }

        private string ComputeSha256Hash(string rawData)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(rawData));
                StringBuilder builder = new StringBuilder();
                foreach (var b in bytes)
                    builder.Append(b.ToString("x2"));
                return builder.ToString();
            }
        }
    }
}
