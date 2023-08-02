using System.Security.Cryptography;

namespace Calendar_Api.Services;

public class Password
{
    public string EncryptPassword(string password)
    {
        var sal = new byte[16];
        using (var rng = RandomNumberGenerator.Create())
        {
            rng.GetBytes(sal);
        }

        var pbkdf2 = new Rfc2898DeriveBytes(password, sal, 10000, HashAlgorithmName.SHA256);
        var hash = pbkdf2.GetBytes(20);

        var hashBytes = new byte[36];
        Array.Copy(sal, 0, hashBytes, 0, 16);
        Array.Copy(hash, 0, hashBytes, 16, 20);

        return Convert.ToBase64String(hashBytes);
    }

    public bool CheckPassword(string password,string confirm)
    {
        var hashBytes = Convert.FromBase64String(password);

        var sal = new byte[16];
        Array.Copy(hashBytes, 0, sal, 0, 16);
        var hash = new byte[20];
        Array.Copy(hashBytes, 16, hash, 0, 20);

        var pbkdf2 = new Rfc2898DeriveBytes(confirm, sal, 10000, HashAlgorithmName.SHA256);
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