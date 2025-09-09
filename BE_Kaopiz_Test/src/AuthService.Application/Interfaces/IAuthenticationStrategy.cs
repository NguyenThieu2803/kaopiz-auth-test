using AuthenticationService.Application.DTOs;
using AuthenticationService.Domain.Entities;
using AuthenticationService.Domain.Enums;

namespace AuthService.Application.Interfaces;

public interface IAuthenticationStrategy
{
    Task<bool> ValidateCredentialsAsync(User user, LoginRequest request);
    Task<AuthResponse> CreateAuthResponseAsync(User user, LoginRequest request);
    bool CanHandle(UserType userType);
}