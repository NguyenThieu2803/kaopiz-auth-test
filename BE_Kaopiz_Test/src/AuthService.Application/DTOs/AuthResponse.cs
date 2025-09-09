namespace AuthenticationService.Application.DTOs;

public class AuthResponse
{
    public string Username { get; set; } = "";
    public string AccessToken { get; set; } = "";
    public DateTime AccessTokenExpiresAt { get; set; }
    public string? RefreshToken { get; set; }
    public DateTime? RefreshTokenExpiresAt { get; set; }
}
