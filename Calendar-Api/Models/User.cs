using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Calendar_Api.Network;

namespace Calendar_Api.Models;

public class User
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    public string Name { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string Password { get; set; } = null!;

    public AuthResponse AuthResponse(string token) => new(this.Id, this.Name, token);

    public (bool, string) ValidUser()
    {
        if (string.IsNullOrEmpty(this.Name)) return (false, "Name is required");
        if (string.IsNullOrEmpty(this.Email)) return (false, "Email is required");
        if (string.IsNullOrEmpty(this.Password) || this.Password.Length < 5)
            return (false, "Password is required or invalid");

        return (true, "");
    }
}