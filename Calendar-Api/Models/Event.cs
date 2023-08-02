using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Calendar_Api.Network;

namespace Calendar_Api.Models;

public class Event
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    public string Title { get; set; }

    public string Notes { get; set; }

    public DateTime Start { get; set; }

    public DateTime End { get; set; }

    [ForeignKey("User")] public int IdUser { get; set; }

    public User User { get; set; } = null!;

    public Event(string title, string notes, DateTime start, DateTime end, int idUser)
    {
        this.Title = title;
        this.Notes = notes;
        this.Start = start;
        this.End = end;
        this.IdUser = idUser;
    }

    public EventResponse EventResponse() =>
        new EventResponse(this.Id, this.Title, this.Notes, this.Start, this.End, this.IdUser);
}