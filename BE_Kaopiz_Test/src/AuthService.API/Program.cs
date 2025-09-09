using AuthenticationService.Application.Interfaces;  // ← Cho IAuthService
using AuthenticationService.Application.Services;
using AuthenticationService.Infrastructure.Data;
using AuthenticationService.Infrastructure.Auth;
using Microsoft.EntityFrameworkCore;
using AuthService.Application.Interfaces;
using AuthenticationService.Application.Strategies;
using AuthService.Application.Strategies;
var builder = WebApplication.CreateBuilder(args);
// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers();
builder.Services.AddScoped<IAuthService, AuthenService>();
builder.Services.AddScoped<JwtTokenService>();
builder.Services.AddDbContext<AppDbContext>(opt =>
    opt.UseSqlServer(builder.Configuration.GetConnectionString("Default")));
builder.Services.AddScoped<IAuthenticationStrategy, EndUserAuthStrategy>();
builder.Services.AddScoped<IAuthenticationStrategy, AdminAuthStrategy>();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.MapControllers();

app.Run();

