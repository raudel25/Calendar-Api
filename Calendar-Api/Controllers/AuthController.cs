using Microsoft.AspNetCore.Mvc;
using Calendar_Api.Models;
using Calendar_Api.Services;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using Calendar_Api.Helpers;

namespace Calendar_Api.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly CalendarContext _context;

    private readonly IConfiguration _configuration;

    public AuthController(CalendarContext context, IConfiguration configuration)
    {
        this._context = context;
        this._configuration = configuration;
    }

    private string Jwt(int id, string name)
    {
        var issuer = _configuration["Jwt:Issuer"];
        var audience = _configuration["Jwt:Audience"];
        var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Key"]!);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.NameIdentifier, id.ToString()),
                new Claim(ClaimTypes.Name, name)
            }),
            Expires = DateTime.UtcNow.AddHours(2),
            Issuer = issuer,
            Audience = audience,
            SigningCredentials =
                new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha512Signature)
        };
        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescriptor);
        var jwtToken = tokenHandler.WriteToken(token);

        return jwtToken;
    }

    [HttpGet("login")]
    public ActionResult<AuthResponse> Login(AuthRequest request)
    {
        var user = _context.Users.SingleOrDefault(u => u.Email == request.Email);

        if (user is null || !user.CheckPassword(request.Password))
            return BadRequest(new { msg = "Incorrect email or password" });

        return user.AuthResponse(Jwt(user.Id, user.Name));
    }

    [HttpPost("register")]
    public ActionResult<AuthResponse> Register(User user)
    {
        var (valid, msg) = user.ValidUser();

        if (!valid) return BadRequest(new { msg });

        if (this._context.Users.SingleOrDefault(u => u.Email == user.Email) is not null)
            return BadRequest(new { msg = "The email is already registered" });

        user.EncryptPassword();

        _context.Users.Add(user);
        _context.SaveChanges();

        return user.AuthResponse(Jwt(user.Id, user.Name));
    }

    [HttpGet("renew"), Authorize]
    public ActionResult<AuthResponse> ReNew()
    {
        var authHeader = Request.Headers["Authorization"].ToString();
        if (!authHeader.StartsWith("Bearer "))
            return BadRequest(new { msg = "Token not found" });

        var (id, name) = Utils.TokenToIdName(authHeader);

        if (id == -1 && name == "") return BadRequest(new { msg = "Token not found" });

        return new AuthResponse(id, name, Jwt(id, name));
    }
}