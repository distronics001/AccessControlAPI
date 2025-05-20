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
                return Unauthorized(new { status = "ACCESS_DENIED" });

            return Ok(new { status = "ACCESS_GRANTED" });
        }
        [HttpPost("register")]
        public IActionResult Register([FromBody] LoginRequest request)
        {
            // Vérifie si l'utilisateur existe déjà
            if (_context.Clients.Any(c => c.Username == request.Username))
                return BadRequest(new { message = "USERNAME_ALREADY_EXISTS" });

            // Hache le mot de passe
            string hashedPassword = ComputeSha256Hash(request.Password);

            // Crée un nouvel utilisateur
            var client = new Client
            {
                Username = request.Username,
                PasswordHash = hashedPassword,
                IsActive = true,
                CreatedAt = DateTime.UtcNow,
                ExpiryDate = DateTime.UtcNow.AddYears(2) // ou une autre logique
            };

            _context.Clients.Add(client);
            _context.SaveChanges();

            return Ok(new { message = "USER_CREATED" });
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
