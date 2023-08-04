using Microsoft.AspNetCore.Mvc;
using Calendar_Api.Models;
using Calendar_Api.Services;
using Microsoft.AspNetCore.Authorization;
using Calendar_Api.Network;
using Microsoft.EntityFrameworkCore;

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
    public async Task<ActionResult<AuthResponse>> Login(AuthRequest request)
    {
        var user = await _context.Users.SingleOrDefaultAsync(u => u.Email == request.Email);

        if (user is null) return BadRequest(new { msg = "Incorrect email" });

        var password = user.Password;

        if (!this._password.CheckPassword(password, request.Password))
            return BadRequest(new { msg = "Incorrect password" });

        return user.AuthResponse(this._token.Jwt(user.Id, user.Name));
    }

    [HttpPost("register")]
    public async Task<ActionResult<AuthResponse>> Register(User user)
    {
        var (valid, msg) = user.ValidUser();

        if (!valid) return BadRequest(new { msg });

        if (await this._context.Users.SingleOrDefaultAsync(u => u.Email == user.Email) is not null)
            return BadRequest(new { msg = "The email is already registered" });

        user.Password = this._password.EncryptPassword(user.Password);

        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        return user.AuthResponse(this._token.Jwt(user.Id, user.Name));
    }

    [HttpGet("renew"), Authorize]
    public ActionResult<AuthResponse> ReNew([FromHeader] string authorization)
    {
        var (id, name) = this._token.TokenToIdName(authorization);

        if (id == -1 && name == "") return BadRequest(new { msg = "Token not found" });

        return new AuthResponse(id, name, this._token.Jwt(id, name));
    }
}