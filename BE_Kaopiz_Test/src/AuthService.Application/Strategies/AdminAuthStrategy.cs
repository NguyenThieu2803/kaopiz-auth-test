
using AuthenticationService.Application.DTOs;
using AuthenticationService.Domain.Entities;
using AuthenticationService.Domain.Enums;
using AuthenticationService.Infrastructure.Auth;
using AuthenticationService.Infrastructure.Data;
using AuthService.Application.Interfaces;

namespace AuthService.Application.Strategies;

public class AdminAuthStrategy : IAuthenticationStrategy
{
    private readonly JwtTokenService _jwt;
    private readonly AppDbContext _db;

    public AdminAuthStrategy(JwtTokenService jwt, AppDbContext db)
    {
        _jwt = jwt;
        _db = db;
    }

    public bool CanHandle(UserType userType) => userType == UserType.Admin;

    public async Task<bool> ValidateCredentialsAsync(User user, LoginRequest request)
    {
        
        // Kiểm tra admin chỉ login trong giờ hành chính
        var currentHour = DateTime.Now.Hour;
        if (currentHour < 8 || currentHour > 18)
        {
            throw new UnauthorizedAccessException("Admin can only login during business hours (8AM-6PM)");
        }

        return true;
    }

    public async Task<AuthResponse> CreateAuthResponseAsync(User user, LoginRequest request)
    {
        var (accessToken, accessExpires) = _jwt.GenerateToken(user);
        
        // Admin: refresh token ngắn hơn (3 ngày)
        string? refreshToken = null;
        DateTime? refreshExpires = null;
        
        if (request.RememberMe)
        {
            refreshToken = Guid.NewGuid().ToString();
            refreshExpires = DateTime.UtcNow.AddDays(3); // Ngắn hơn end-user
            
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
            UserType = user.Role.ToString(),
        };
    }
}