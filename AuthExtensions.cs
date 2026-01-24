using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace Ads;

public static class AuthExtensions
{
    public static IServiceCollection AddAuth(this IServiceCollection serviceCollection, IConfiguration configuration)
    {
        var authSettings = configuration.GetSection("AuthSettings").Get<AuthSettings>();
        serviceCollection.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(o =>
                {
                    o.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = false,
                        ValidateAudience = false,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey =
                            new SymmetricSecurityKey(Encoding.UTF8.GetBytes(authSettings.SecretKey))
                    };
                }
            );
        return serviceCollection;
    }
}