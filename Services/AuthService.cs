using Ads.DTO;
using Ads.Models;
using Ads.Repositories.Interfaces;
using Ads.Services.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace Ads.Services;

public class AuthService(IUserRepository userRepository, IPasswordHasher<User> hasher, JwtService jwtService)
    : IAuthService
{
    public async Task RegisterAsync(RegisterRequest request)
    {
        if (await userRepository.ExistsByEmailAsync(request.Email))
            throw new Exception("Email already exists");

        var user = new User
        {
            Name = request.Name,
            Email = request.Email,
            RegistrationDate = DateTime.UtcNow
        };
        user.PasswordHash = hasher.HashPassword(null!, request.Password);
        
        await userRepository.AddAsync(user);
    }

    public async Task<string> LoginAsync(LoginRequest request)
    {
        var user = await userRepository.GetByEmailAsync(request.Email);
        
        if (user == null)
            throw new Exception("Invalid email or password");
        
        var isValid = hasher.VerifyHashedPassword(user, user.PasswordHash, request.Password);
        if (isValid == PasswordVerificationResult.Failed)
            throw new Exception("Invalid email or password");
        
        return jwtService.GenerateToken(user);
    }
}