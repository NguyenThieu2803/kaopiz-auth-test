using AuthenticationService.Domain.Enums;

namespace AuthenticationService.Application.DTOs;

public class RegisterRequest
{
    public string Username { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string Password { get; set; } = null!;
    public UserType UserType { get; set; } = UserType.User;
}
