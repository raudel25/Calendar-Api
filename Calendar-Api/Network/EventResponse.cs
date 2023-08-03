namespace Calendar_Api.Network;

public class EventResponse
{
    public int Id { get; set; }

    public string Title { get; set; }

    public string Notes { get; set; }

    public long Start { get; set; }

    public long End { get; set; }

    public int IdUser { get; set; }

    public EventResponse(int id, string title, string notes, long start, long end, int idUser)
    {
        this.Id = id;
        this.Title = title;
        this.Notes = notes;
        this.Start = start;
        this.End = end;
        this.IdUser = idUser;
    }
}