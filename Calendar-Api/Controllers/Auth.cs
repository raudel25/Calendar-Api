using Microsoft.AspNetCore.Mvc;
using Calendar_Api.Models;
using Calendar_Api.Services;

namespace Calendar_Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class Auth : ControllerBase
{
    private readonly CalendarContext _context;

    public Auth(CalendarContext context)
    {
        this._context = context;
    }

    [HttpGet]
    public ActionResult<UserInf> Login(AuthRequest request)
    {
        var actUser = _context.Users.SingleOrDefault(u => u.Email == request.Email);

        if (actUser is null || !actUser.CheckPassword(request.Password))
            return BadRequest(new { msg = "Incorrect email or password" });

        return actUser.UserInf();
    }

    [HttpPost("register")]
    public ActionResult<User> Register(User user)
    {
        var (valid, msg) = user.ValidUser();

        if (!valid) return BadRequest(new { msg });

        if (this._context.Users.SingleOrDefault(u => u.Email == user.Email) is not null)
            return BadRequest(new { msg = "The email is already registered" });

        user.EncryptPassword();

        _context.Users.Add(user);
        _context.SaveChanges();

        return CreatedAtAction(nameof(Login), user.UserInf());
    }

    [HttpGet("renew")]
    public IActionResult ReNew()
    {
        return Ok(new { msg = "renew" });
    }
}