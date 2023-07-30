namespace Calendar_Api.Helpers;

public class AuthRequest
{
    public string Email { get; set; }
    public string Password { get; set; }

    public AuthRequest(string email, string password)
    {
        this.Email = email;
        this.Password = password;
    }
}