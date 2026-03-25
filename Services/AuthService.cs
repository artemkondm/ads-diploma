using Ads.DTO;
using Ads.Models;
using Ads.Repositories.Interfaces;
using Ads.Services.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace Ads.Services;

public class AuthService : IAuthService
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHasher<User> _hasher;
    private readonly JwtService _jwtService;

    public AuthService(IUserRepository userRepository, IPasswordHasher<User> hasher, JwtService jwtService)
    {
        _userRepository = userRepository;
        _hasher = hasher;
        _jwtService = jwtService;
    }
    public async Task RegisterAsync(RegisterRequest request)
    {
        if (await _userRepository.ExistsByEmailAsync(request.Email))
            throw new Exception("Email already exists");

        var user = new User
        {
            Name = request.Name,
            Email = request.Email,
            RegistrationDate = DateTime.UtcNow
        };
        user.PasswordHash = _hasher.HashPassword(null!, request.Password);
        
        await _userRepository.AddAsync(user);
    }

    public async Task<string> LoginAsync(LoginRequest request)
    {
        var user = await _userRepository.GetByEmailAsync(request.Email);
        
        if (user == null)
            throw new Exception("Invalid email or password");
        
        var isValid = _hasher.VerifyHashedPassword(user, user.PasswordHash, request.Password);
        if (isValid == PasswordVerificationResult.Failed)
            throw new Exception("Invalid email or password");
        
        return _jwtService.GenerateToken(user);
    }
}