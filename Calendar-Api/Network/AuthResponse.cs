namespace Calendar_Api.Network;

public class AuthResponse
{
    public int Id { get; set; }

    public string Name { get; set; }
    
    public string Token { get; set; }

    public AuthResponse(int id, string name,string token)
    {
        this.Id = id;
        this.Name = name;
        this.Token = token;
    }
}