using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using TaskApp.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using System.Security.Claims;

[ApiController]
[Route("api/[controller]")]
public class LoginController : ControllerBase
{
    private readonly IConfiguration _config;
    private readonly ModelContext _context;

    public LoginController(IConfiguration config, ModelContext context)
    {
        _config = config;
        _context = context;
    }

    [HttpPost("login")]
    public IActionResult Login([FromBody] LoginRequest login)
    {
        if (login == null || string.IsNullOrEmpty(login.Username) || string.IsNullOrEmpty(login.Password))
        {
            return BadRequest("Username and Password are required.");
        }

        // Find the user from RSUSERINFO
        var user = _context.Rsuserinfos
            .FirstOrDefault(u => u.Username == login.Username && u.Isactive == true);

        if (user == null)
        {
            return Unauthorized("User not found or not active.");
        }

        // Simple password check (in real app, use hashing like BCrypt)
        if (user.Passwordhash != login.Password)
        {
            return Unauthorized("Invalid password.");
        }

        // If valid → Generate JWT
        var token = GenerateJwtToken(user.Username);
        return Ok(new
        {
            Message = "Login successful",
            Token = token,
            Username = user.Username,
            UserId = user.Userid
        });
    }

    private string GenerateJwtToken(string username)
    {
        var securityKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim(ClaimTypes.Name, username)
        };

        var token = new JwtSecurityToken(
            issuer: _config["Jwt:Issuer"],
            audience: _config["Jwt:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddHours(1),
            signingCredentials: credentials);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
