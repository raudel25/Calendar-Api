using Microsoft.AspNetCore.Mvc;
using Calendar_Api.Models;
using Calendar_Api.Services;
using Microsoft.AspNetCore.Authorization;
using Calendar_Api.Network;

namespace Calendar_Api.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly CalendarContext _context;

    private readonly Token _token;

    private readonly Password _password;

    public AuthController(CalendarContext context, Token token, Password password)
    {
        this._context = context;
        this._token = token;
        this._password = password;
    }

    [HttpPost("login")]
    public ActionResult<AuthResponse> Login(AuthRequest request)
    {
        var user = _context.Users.SingleOrDefault(u => u.Email == request.Email);

        if (user is null) return BadRequest(new { msg = "Incorrect email" });

        var password = user.Password;

        if (!this._password.CheckPassword(password, request.Password))
            return BadRequest(new { msg = "Incorrect password" });

        return user.AuthResponse(this._token.Jwt(user.Id, user.Name));
    }

    [HttpPost("register")]
    public ActionResult<AuthResponse> Register(User user)
    {
        var (valid, msg) = user.ValidUser();

        if (!valid) return BadRequest(new { msg });

        if (this._context.Users.SingleOrDefault(u => u.Email == user.Email) is not null)
            return BadRequest(new { msg = "The email is already registered" });

        user.Password = this._password.EncryptPassword(user.Password);

        _context.Users.Add(user);
        _context.SaveChanges();

        return user.AuthResponse(this._token.Jwt(user.Id, user.Name));
    }

    [HttpGet("renew"), Authorize]
    public ActionResult<AuthResponse> ReNew()
    {
        var authHeader = Request.Headers["Authorization"].ToString();

        var (id, name) = this._token.TokenToIdName(authHeader);

        if (id == -1 && name == "") return BadRequest(new { msg = "Token not found" });

        return new AuthResponse(id, name, this._token.Jwt(id, name));
    }
}