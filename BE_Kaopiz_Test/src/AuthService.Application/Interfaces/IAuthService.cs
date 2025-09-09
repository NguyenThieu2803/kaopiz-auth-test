using AuthenticationService.Application.DTOs;

namespace AuthenticationService.Application.Interfaces;



public interface IAuthService
{
    Task<AuthResponse> RegisterAsync(RegisterRequest request);
    Task<AuthResponse> LoginAsync(LoginRequest request);
    Task  LogoutAsync(string refreshToken);
}