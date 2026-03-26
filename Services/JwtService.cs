using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Ads.Models;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Ads.Services;

public class JwtService(IOptions<AuthSettings> options)
{
    private readonly AuthSettings _options = options.Value;

    public string GenerateToken(User user)
    {
        var claims = new List<Claim>()
        {
            new Claim("email", user.Email),
            new Claim("userId", user.Id.ToString())
        };

        var jwtToken = new JwtSecurityToken(
            expires: DateTime.UtcNow.Add(_options.Expires),
            claims: claims,
            signingCredentials: new SigningCredentials(
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.SecretKey)),
                SecurityAlgorithms.HmacSha256));
        
        return  new JwtSecurityTokenHandler().WriteToken(jwtToken);
    }
}