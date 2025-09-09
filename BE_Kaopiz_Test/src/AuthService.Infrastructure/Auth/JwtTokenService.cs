
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AuthenticationService.Domain.Entities;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
namespace AuthenticationService.Infrastructure.Auth;

public class JwtTokenService
{
    private readonly IConfiguration _config;

    public JwtTokenService(IConfiguration config)
    {
        _config = config;
    }

    public (string token, DateTime expires) GenerateToken(User user)
    {
        var issuer = _config["Jwt:Issuer"];
        var audience = _config["Jwt:Audience"];
        var key = _config["Jwt:Key"];
        var lifetime = int.Parse(_config["Jwt:AccessTokenMinutes"] ?? "30");

        //claims là những thông tin về user mà ta muốn nhét vào token
        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.UniqueName, user.Username),
            new Claim("email", user.Email)
        };
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key!));
        var creds = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
        
        var expires = DateTime.UtcNow.AddMinutes(lifetime);
        
        //nhét thông tin vào token
        var token = new JwtSecurityToken(
            issuer: issuer,
            audience: audience,
            claims: claims,
            expires: expires,
            signingCredentials: creds
        );
        
        var tokenString = new JwtSecurityTokenHandler().WriteToken(token);
        return (tokenString, expires);
    }
}
