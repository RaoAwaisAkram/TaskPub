using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaskApp.Models;
using Microsoft.EntityFrameworkCore;
namespace TaskApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RegisterUserController : ControllerBase
    {
        private readonly ModelContext _context;

        public RegisterUserController(ModelContext context)
        {
            _context = context;
        }

        [HttpPost("RegisterUser")]
        public async Task<IActionResult> PostData([FromBody] Rsuserinfo regUser)
        {
            if (string.IsNullOrEmpty(regUser.Username))
            {
                return BadRequest("User Name is required");
            }

            // Check for duplicate username
            var existingUser = await _context.Rsuserinfos.FirstOrDefaultAsync(u => u.Username == regUser.Username);
            if (existingUser != null)
            {
                return Conflict("Username already exists.");
            }

            regUser.Creadtets = DateTime.UtcNow;
            regUser.Isactive = true;

            _context.Rsuserinfos.Add(regUser);
            await _context.SaveChangesAsync();

            return Ok(new { Message = "User registered successfully", regUser.Userid });
        }
    }
}
