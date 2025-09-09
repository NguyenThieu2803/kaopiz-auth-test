using AuthenticationService.Application.DTOs;
using AuthenticationService.Application.Interfaces;
using AuthenticationService.Domain.Entities;
using AuthenticationService.Infrastructure.Data;
namespace AuthenticationService.Application.Services;

using System.Security.Cryptography;
using System.Text;
using AuthenticationService.Infrastructure.Auth;
using AuthService.Application.Interfaces;
using Microsoft.EntityFrameworkCore;
public class AuthenService : IAuthService
{
    private readonly AppDbContext _db;
    private readonly JwtTokenService _jwt;
    private readonly IEnumerable<IAuthenticationStrategy> _authStrategies;

    public AuthenService(AppDbContext db, JwtTokenService jwt, IEnumerable<IAuthenticationStrategy> authStrategies)
    {
        _db = db;
        _jwt = jwt;
        _authStrategies = authStrategies;
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
            Role = request.UserType,
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

         // Tìm strategy phù hợp với user type và mỗi loại user sẽ có một cách xử lý khác nhau
        var strategy = _authStrategies.FirstOrDefault(s => s.CanHandle(user.Role));
        if (strategy == null)
            throw new NotSupportedException($"Authentication not supported for user type: {user.Role}");

        // Validate theo strategy
        await strategy.ValidateCredentialsAsync(user, request);

        // Tạo response theo strategy
        return await strategy.CreateAuthResponseAsync(user, request);
    }
    public async Task LogoutAsync(string refreshToken)
    {
        var tokenEntity = await _db.RefreshTokens.SingleOrDefaultAsync(t => t.Token == refreshToken);
        if (tokenEntity != null)
        {
            _db.RefreshTokens.Remove(tokenEntity);
            await _db.SaveChangesAsync();
        }
        // Nếu không tìm thấy cũng không báo lỗi để tránh lộ thông tin
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
