using Calendar_Api.Models;

namespace Calendar_Api.Helpers;

public class EventRequest
{
    public string Title { get; set; }

    public string Notes { get; set; }

    public DateTime Start { get; set; }

    public DateTime End { get; set; }

    public Event Event(int idUser) => new Event(this.Title, this.Notes, this.Start, this.End, idUser);
}