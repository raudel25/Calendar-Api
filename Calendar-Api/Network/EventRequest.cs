using Calendar_Api.Models;

namespace Calendar_Api.Network;

public class EventRequest
{
    public string Title { get; set; } = null!;

    public string Notes { get; set; } = null!;

    public long Start { get; set; }

    public long End { get; set; }

    public Event Event(int idUser) => new Event(this.Title, this.Notes, this.Start, this.End, idUser);
}