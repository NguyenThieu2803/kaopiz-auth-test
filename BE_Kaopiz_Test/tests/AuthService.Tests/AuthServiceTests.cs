using AuthenticationService.Application.DTOs;
using AuthenticationService.Application.Interfaces;
using AuthenticationService.Application.Services;
using AuthenticationService.Domain.Entities;
using AuthenticationService.Domain.Enums;
using AuthenticationService.Infrastructure.Auth;
using AuthenticationService.Infrastructure.Data;
using AuthService.Application.Interfaces;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Moq;
namespace AuthService.Tests.Services;

public class AuthServiceTests : IDisposable // giúp giải phóng tài nguyên sau khi test xong
{

    private readonly AppDbContext _context;
    private readonly AuthenService _authService;
    private readonly Mock<JwtTokenService> _mockJwtService;
    private readonly List<Mock<IAuthenticationStrategy>> _mockStrategies;// Mock các strategy


    public AuthServiceTests()
    {
        
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()) // Dùng db giả trong ram -> nhanh, ko cần sql server tránh cònig
            .Options;
        
        _context = new AppDbContext(options);
        
        //tạo mock cho service
        var mockConfig = new Mock<IConfiguration>();
        _mockJwtService = new Mock<JwtTokenService>(mockConfig.Object);
        
        
        _mockStrategies = new List<Mock<IAuthenticationStrategy>>
        {
            new Mock<IAuthenticationStrategy>(),
            new Mock<IAuthenticationStrategy>(),
        };
        //tạo mock cho IAuthenticationStrategy
        var strategies = _mockStrategies.Select(m => m.Object);
        
        _authService = new AuthenService(_context, _mockJwtService.Object, strategies);
    }
    [Fact]
    public async Task RegisterAsync_WithValidRequest_ShouldCreateUserSuccessfully()
    {
        var request = new RegisterRequest
        {
            Username = "newuser",
            Email = "newuser@example.com",
            Password = "SecurePassword123!",
            UserType = UserType.User
        };

        // Mock JWT service để trả về token giả
        _mockJwtService.Setup(j => j.GenerateToken(It.IsAny<User>()))
            .Returns(("fake-jwt-token-12345", DateTime.UtcNow.AddMinutes(30)));

        var result = await _authService.RegisterAsync(request);

        
        // 1. Kiểm tra response
        result.Should().NotBeNull("Service should return a response");
        result.Username.Should().Be("newuser", "Response should contain correct username");
        result.AccessToken.Should().Be("fake-jwt-token-12345", "Response should contain generated token");
        result.AccessTokenExpiresAt.Should().BeAfter(DateTime.UtcNow, "Token should have future expiry");

        // 2. Kiểm tra user đã được lưu vào database
        var userInDb = await _context.Users.FirstOrDefaultAsync(u => u.Username == "newuser");
        userInDb.Should().NotBeNull("User should be saved to database");
        userInDb!.Email.Should().Be("newuser@example.com", "User email should be saved correctly");
        userInDb.Role.Should().Be(UserType.User, "User role should be saved correctly");

        // 3. Kiểm tra password đã được hash
        userInDb.PasswordHash.Should().NotBeEmpty("Password should be hashed");
        userInDb.PasswordSalt.Should().NotBeEmpty("Password salt should be generated");
        
        // 4. Verify JWT service được gọi đúng cách
        _mockJwtService.Verify(j => j.GenerateToken(It.Is<User>(u => u.Username == "newuser")), 
            Times.Once, "JWT service should be called once with correct user");
    }

    [Fact]
    public async Task RegisterAsync_WithExistingUsername_ShouldThrowException()
    {

        // Tạo user đã tồn tại trong database
        var existingUser = CreateTestUser("existinguser", "existing@example.com");
        _context.Users.Add(existingUser);
        await _context.SaveChangesAsync();

        var request = new RegisterRequest
        {
            Username = "existinguser", // Trùng username
            Email = "different@example.com",
            Password = "Password123!",
            UserType = UserType.User
        };

        var exception = await Assert.ThrowsAsync<Exception>(() => _authService.RegisterAsync(request));
        
        exception.Message.Should().Be("Username already exists", 
            "Should throw specific error message for duplicate username");
            
        // Verify JWT service KHÔNG được gọi khi có lỗi
        _mockJwtService.Verify(j => j.GenerateToken(It.IsAny<User>()), 
            Times.Never, "JWT service should not be called when registration fails");
    }

    [Fact]
    public async Task LoginAsync_WithValidCredentials_ShouldReturnAuthResponse()
    {

        // Tạo user test với password đã hash
        var user = CreateTestUserWithHashedPassword("testuser", "test@example.com", "Password123!");
        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        var request = new LoginRequest
        {
            Username = "testuser",
            Password = "Password123!",
            RememberMe = false
        };

        // Setup strategy mock
        var strategyMock = _mockStrategies[0];
        strategyMock.Setup(s => s.CanHandle(UserType.User)).Returns(true);
        strategyMock.Setup(s => s.ValidateCredentialsAsync(It.IsAny<User>(), It.IsAny<LoginRequest>()))
            .Returns(Task.FromResult(true));
        strategyMock.Setup(s => s.CreateAuthResponseAsync(It.IsAny<User>(), It.IsAny<LoginRequest>()))
            .ReturnsAsync(new AuthResponse
            {
                Username = "testuser",
                AccessToken = "strategy-generated-token",
                AccessTokenExpiresAt = DateTime.UtcNow.AddMinutes(30)
            });

        var result = await _authService.LoginAsync(request);

        result.Should().NotBeNull("Login should return response");
        result.Username.Should().Be("testuser", "Response should contain correct username");
        result.AccessToken.Should().Be("strategy-generated-token", "Should use strategy-generated token");
        
        // Verify strategy methods được gọi đúng thứ tự
        strategyMock.Verify(s => s.CanHandle(UserType.User), Times.AtLeastOnce,
            "Strategy should check if it can handle user type");
        strategyMock.Verify(s => s.ValidateCredentialsAsync(It.IsAny<User>(), request), Times.Once, 
            "Strategy should validate credentials");
        strategyMock.Verify(s => s.CreateAuthResponseAsync(It.IsAny<User>(), request), Times.Once, 
            "Strategy should create auth response");
    }

    [Theory] //cho phép test với nhiều data khác nhau
    [InlineData("testuser", "WrongPassword")]
    [InlineData("NonExistentUser", "Password123!")]
    public async Task LoginAsync_WithInvalidCredentials_ShouldThrowException(string username, string password)
    {
        var user = CreateTestUserWithHashedPassword("testuser", "test@example.com", "Password123!");
        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        var request = new LoginRequest
        {
            Username = username,
            Password = password,
            RememberMe = false
        };

        var exception = await Assert.ThrowsAsync<Exception>(() => _authService.LoginAsync(request));
        exception.Message.Should().Be("Invalid credentials", 
            "Should throw specific error for invalid login");
    }

    [Fact]
    public async Task LoginAsync_WithUnsupportedUserType_ShouldThrowNotSupportedException()
    {

        var user = CreateTestUserWithHashedPassword("testuser", "test@example.com", "Password123!");
        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        var request = new LoginRequest
        {
            Username = "testuser",
            Password = "Password123!",
            RememberMe = false
        };

        // Tất cả strategies đều trả về false cho CanHandle
        foreach (var strategyMock in _mockStrategies)
        {
            strategyMock.Setup(s => s.CanHandle(It.IsAny<UserType>())).Returns(false);
        }


        var exception = await Assert.ThrowsAsync<NotSupportedException>(() => _authService.LoginAsync(request));
        exception.Message.Should().Contain("Authentication not supported", 
            "Should throw specific error for unsupported user type");
    }

    private User CreateTestUser(string username, string email)
    {
        return new User
        {
            Username = username,
            Email = email,
            Role = UserType.User,
            PasswordHash = new byte[64], // Dummy hash
            PasswordSalt = new byte[128] // Dummy salt
        };
    }

    private User CreateTestUserWithHashedPassword(string username, string email, string password)
    {
        using var hmac = new System.Security.Cryptography.HMACSHA512();
        var salt = hmac.Key;
        var hash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));

        return new User
        {
            Username = username,
            Email = email,
            Role = UserType.User,
            PasswordHash = hash,
            PasswordSalt = salt
        };
    }
    public void Dispose()
    {
        _context.Dispose();
    }
}