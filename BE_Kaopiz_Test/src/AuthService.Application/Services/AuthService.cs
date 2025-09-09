using AuthenticationService.Application.DTOs;
using AuthenticationService.Application.Interfaces;
using AuthenticationService.Domain.Entities;
using AuthenticationService.Infrastructure.Data;
using AuthenticationService.Infrastructure.Auth;
namespace AuthenticationService.Application.Services;

using System.Security.Cryptography;
using System.Text;
using Microsoft.EntityFrameworkCore;
public class AuthenService : IAuthService
{
    private readonly AppDbContext _db;
    private readonly JwtTokenService _jwt;

    public AuthenService(AppDbContext db, JwtTokenService jwt)
    {
        _db = db;
        _jwt = jwt;
    }

    public async Task<AuthResponse> RegisterAsync(RegisterRequest request)
    {
        if (await _db.Users.AnyAsync(u => u.Username == request.Username))
            throw new Exception("Username already exists");

        CreatePasswordHash(request.Password, out byte[] hash, out byte[] salt);
        var user = new User
        {
            Username = request.Username,
            Email = request.Email,
            PasswordHash = hash,
            PasswordSalt = salt
        };

        _db.Users.Add(user);
        await _db.SaveChangesAsync();

        var (token, expires) = _jwt.GenerateToken(user);
        return new AuthResponse
        {
            Username = user.Username,
            AccessToken = token,
            AccessTokenExpiresAt = expires
        };
    }

    public async Task<AuthResponse> LoginAsync(LoginRequest request)
    {
        var user = await _db.Users.Include(u => u.RefreshTokens)
            .SingleOrDefaultAsync(u => u.Username == request.Username);

        if (user == null || !VerifyPasswordHash(request.Password, user.PasswordHash, user.PasswordSalt))
            throw new Exception("Invalid credentials");

        var (accessToken, accessExpires) = _jwt.GenerateToken(user);
        string? refreshToken = "";
        DateTime? refreshExpires = null;
        if (request.RememberMe)
        {
            // Tạo refresh token mới
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
            RefreshTokenExpiresAt = refreshExpires
        };
    }


    private void CreatePasswordHash(string password, out byte[] hash, out byte[] salt)
    {
        using var hmac = new HMACSHA512();
        salt = hmac.Key;
        hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
    }

    private bool VerifyPasswordHash(string password, byte[] hash, byte[] salt)
    {
        using var hmac = new HMACSHA512(salt);
        var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
        return computedHash.SequenceEqual(hash);
    }
}
