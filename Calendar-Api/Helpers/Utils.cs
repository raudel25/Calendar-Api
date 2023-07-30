using Microsoft.AspNetCore.Mvc;
using Calendar_Api.Models;
using Calendar_Api.Services;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using Calendar_Api.Helpers;

namespace Calendar_Api.Helpers;

public static class Utils
{
    public static (int, string) TokenToIdName(string authHeader)
    {
        var token = authHeader.Substring("Bearer ".Length).Trim();

        var handler = new JwtSecurityTokenHandler();
        var jwtToken = handler.ReadJwtToken(token);

        var idClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == "nameid");
        var nameClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == "unique_name");

        if (idClaim is null || nameClaim is null) return (-1, "");

        return (int.Parse(idClaim.Value), nameClaim.Value);
    }
}