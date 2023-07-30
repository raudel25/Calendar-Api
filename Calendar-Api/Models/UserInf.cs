namespace Calendar_Api.Models;

public class UserInf
{
    public int Id { get; set; }

    public string Name { get; set; }

    public UserInf(int id, string name)
    {
        this.Id = id;
        this.Name = name;
    }
}