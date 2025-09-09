using AuthenticationService.Application.DTOs;
using AuthenticationService.Application.Interfaces;
using AuthenticationService.Domain.Entities;
using AuthenticationService.Domain.Enums;
using AuthenticationService.Infrastructure.Auth;
using AuthenticationService.Infrastructure.Data;
using AuthService.Application.Interfaces;
namespace AuthenticationService.Application.Strategies;


public class EndUserAuthStrategy : IAuthenticationStrategy
{
    private readonly JwtTokenService _jwt;
    private readonly AppDbContext _db;

    public EndUserAuthStrategy(JwtTokenService jwt, AppDbContext db)
    {
        _jwt = jwt;
        _db = db;
    }
    public bool CanHandle(UserType userType) => userType == UserType.User;
    public Task<bool> ValidateCredentialsAsync(User user, LoginRequest request)
    {
        return Task.FromResult(true);
    }

    public async Task<AuthResponse> CreateAuthResponseAsync(User user, LoginRequest request)
    {
        var (accessToken, accessExpires) = _jwt.GenerateToken(user);
        
        // End-user: refresh token 7 ngày
        string? refreshToken = null;
        DateTime? refreshExpires = null;
        
        if (request.RememberMe)
        {
            refreshToken = Guid.NewGuid().ToString();
            refreshExpires = DateTime.UtcNow.AddDays(7);
            
            var refreshTokenEntity = new RefreshToken
            {
                Token = refreshToken,
                ExpiresAt = refreshExpires.Value,
                UserId = user.Id
            };
            _db.RefreshTokens.Add(refreshTokenEntity);
            await _db.SaveChangesAsync();
        }

        return new AuthResponse
        {
            Username = user.Username,
            AccessToken = accessToken,
            AccessTokenExpiresAt = accessExpires,
            RefreshToken = refreshToken,
            RefreshTokenExpiresAt = refreshExpires,
            UserType = user.Role.ToString()
        };
    }
}
