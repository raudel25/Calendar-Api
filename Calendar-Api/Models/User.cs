using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Cryptography;

namespace Calendar_Api.Models;

public class User
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    public string Name { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }

    public UserInf UserInf() => new(this.Id, this.Name);

    public (bool, string) ValidUser()
    {
        if (string.IsNullOrEmpty(this.Name)) return (false, "Name is required");
        if (string.IsNullOrEmpty(this.Email)) return (false, "Email is required");
        if (string.IsNullOrEmpty(this.Password) || this.Password.Length < 5)
            return (false, "Password is required or invalid");

        return (true, "");
    }

    public void EncryptPassword()
    {
        var sal = new byte[16];
        using (var rng = RandomNumberGenerator.Create())
        {
            rng.GetBytes(sal);
        }

        var pbkdf2 = new Rfc2898DeriveBytes(this.Password, sal, 10000, HashAlgorithmName.SHA256);
        var hash = pbkdf2.GetBytes(20);

        var hashBytes = new byte[36];
        Array.Copy(sal, 0, hashBytes, 0, 16);
        Array.Copy(hash, 0, hashBytes, 16, 20);

        this.Password = Convert.ToBase64String(hashBytes);
    }

    public bool CheckPassword(string password)
    {
        var hashBytes = Convert.FromBase64String(this.Password);

        var sal = new byte[16];
        Array.Copy(hashBytes, 0, sal, 0, 16);
        var hash = new byte[20];
        Array.Copy(hashBytes, 16, hash, 0, 20);

        var pbkdf2 = new Rfc2898DeriveBytes(password, sal, 10000, HashAlgorithmName.SHA256);
        var hashPass = pbkdf2.GetBytes(20);

        for (var i = 0; i < 20; i++)
        {
            if (hash[i] != hashPass[i])
            {
                return false;
            }
        }

        return true;
    }
}