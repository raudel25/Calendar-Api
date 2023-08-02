using Microsoft.AspNetCore.Mvc;
using Calendar_Api.Services;
using Microsoft.AspNetCore.Authorization;
using Calendar_Api.Network;
using Microsoft.EntityFrameworkCore;

namespace Calendar_Api.Controllers;

[Authorize]
[ApiController]
[Route("api/event")]
public class EventController : ControllerBase
{
    private readonly CalendarContext _context;

    private readonly Token _token;
    public EventController(CalendarContext context, Token token)
    {
        this._context = context;
        this._token = token;
    }

    [HttpGet]
    public ActionResult<IEnumerable<EventResponse>> Get()
    {
        var authHeader = Request.Headers["Authorization"].ToString();
        if (!authHeader.StartsWith("Bearer "))
            return BadRequest(new { msg = "Token not found" });

        var (id, name) = this._token.TokenToIdName(authHeader);

        if (id == -1 && name == "") return BadRequest(new { msg = "Token not found" });

        return this._context.Events.Where(e => e.IdUser == id).Select(e => e.EventResponse()).ToList();
    }

    [HttpGet("{id:int}")]
    public ActionResult<EventResponse> GetEvent(int id)
    {
        var authHeader = Request.Headers["Authorization"].ToString();

        var (idUser, name) = this._token.TokenToIdName(authHeader);

        if (idUser == -1 && name == "") return BadRequest(new { msg = "Token not found" });

        var e = this._context.Events.SingleOrDefault(e => e.IdUser == idUser && e.Id == id);

        return e is null ? NotFound() : e.EventResponse();
    }

    [HttpPost]
    public ActionResult<EventResponse> Post(EventRequest request)
    {
        var authHeader = Request.Headers["Authorization"].ToString();

        var (id, name) = this._token.TokenToIdName(authHeader);

        if (id == -1 && name == "") return BadRequest(new { msg = "Token not found" });

        var e = request.Event(id);

        this._context.Events.Add(e);
        this._context.SaveChanges();

        return e.EventResponse();
    }

    [HttpPut("{id:int}")]
    public ActionResult<EventResponse> Put(int id, EventRequest request)
    {
        var authHeader = Request.Headers["Authorization"].ToString();

        var (idUser, name) = this._token.TokenToIdName(authHeader);

        if (idUser == -1 && name == "") return BadRequest(new { msg = "Token not found" });

        var e = this._context.Events.SingleOrDefault(e => e.IdUser == idUser && e.Id == id);
        if (e is null) return NotFound(new { msg = "User not found" });

        _context.Entry(e).State = EntityState.Detached;

        var updated = request.Event(idUser);
        updated.Id = id;

        this._context.Events.Update(updated);
        this._context.SaveChanges();

        return e.EventResponse();
    }

    [HttpDelete("{id:int}")]
    public IActionResult Delete(int id)
    {
        var authHeader = Request.Headers["Authorization"].ToString();

        var (idUser, name) = this._token.TokenToIdName(authHeader);

        if (idUser == -1 && name == "") return BadRequest(new { msg = "Token not found" });

        var e = this._context.Events.SingleOrDefault(e => e.IdUser == idUser && e.Id == id);
        if (e is null) return NotFound(new { msg = "User not found" });

        this._context.Events.Remove(e);
        this._context.SaveChanges();

        return Ok();
    }
}