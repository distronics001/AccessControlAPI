using Microsoft.AspNetCore.Mvc;
using AccessControlAPI.Models;

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
            var user = _context.Clients.FirstOrDefault(c =>
                c.Username == request.Username &&
                c.PasswordHash == request.Password &&
                c.IsActive &&
                (c.ExpiryDate == null || c.ExpiryDate > DateTime.UtcNow)
            );

            if (user == null)
                return Unauthorized(new { message = "ACCESS_DENIED" });

            return Ok(new { message = "ACCESS_GRANTED" });
        }
    }
}
