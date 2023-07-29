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

    private (bool, string) ValidUser(User user)
    {
        if (string.IsNullOrEmpty(user.Name)) return (false, "Name is required");
        if (string.IsNullOrEmpty(user.Email)) return (false, "Email is required");
        if (string.IsNullOrEmpty(user.Password) || user.Password.Length < 5)
            return (false, "Password is required or invalid");

        return (true, "");
    }

    [HttpGet]
    public ActionResult<User> Login(AuthRequest request)
    {
        var actUser = _context.Users.SingleOrDefault(u => u.Email == request.Email && u.Password == request.Password);

        if (actUser is null) return NotFound();

        return actUser;
    }

    [HttpPost("register")]
    public ActionResult<User> Register(User user)
    {
        var (valid, msg) = ValidUser(user);

        if (!valid) return BadRequest(new { msg });

        _context.Users.Add(user);
        _context.SaveChanges();

        return CreatedAtAction(nameof(Login), new { id = user.Id }, user);
    }

    [HttpGet("renew")]
    public IActionResult ReNew()
    {
        return Ok(new { msg = "renew" });
    }
}