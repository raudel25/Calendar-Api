using System.Text;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;

namespace Calendar_Api.Services;

public class Token
{
    private readonly IConfiguration _configuration;

    public Token(IConfiguration configuration)
    {
        this._configuration = configuration;
    }

    public string Jwt(int id, string name)
    {
        var issuer = _configuration["Jwt:Issuer"];
        var audience = _configuration["Jwt:Audience"];
        var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Key"]!);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.NameIdentifier, id.ToString()),
                new Claim(ClaimTypes.Name, name)
            }),
            Expires = DateTime.UtcNow.AddHours(2),
            Issuer = issuer,
            Audience = audience,
            SigningCredentials =
                new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha512Signature)
        };
        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescriptor);
        var jwtToken = tokenHandler.WriteToken(token);

        return jwtToken;
    }

    public (int, string) TokenToIdName(string authHeader)
    {
        if (!authHeader.StartsWith("Bearer "))
            return (-1, "");

        var token = authHeader.Substring("Bearer ".Length).Trim();

        var handler = new JwtSecurityTokenHandler();
        var jwtToken = handler.ReadJwtToken(token);

        var idClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == "nameid");
        var nameClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == "unique_name");

        if (idClaim is null || nameClaim is null) return (-1, "");

        return (int.Parse(idClaim.Value), nameClaim.Value);
    }
}