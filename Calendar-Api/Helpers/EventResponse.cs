namespace Calendar_Api.Helpers;

public class EventResponse
{
    public int Id { get; set; }

    public string Title { get; set; }

    public string Notes { get; set; }

    public DateTime Start { get; set; }

    public DateTime End { get; set; }

    public int IdUser { get; set; }

    public EventResponse(int id, string title, string notes, DateTime start, DateTime end, int idUser)
    {
        this.Id = id;
        this.Title = title;
        this.Notes = notes;
        this.Start = start;
        this.End = end;
        this.IdUser = idUser;
    }
}