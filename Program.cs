using Ads;
using Ads.Database;
using Ads.Models;
using Ads.Repositories;
using Ads.Repositories.Interfaces;
using Ads.Services;
using Ads.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IAdsService, AdsService>();
builder.Services.AddScoped<IProfileService, ProfileService>();
builder.Services.AddHttpClient<IGeoService, GeoService>()
    .ConfigureHttpClient(c =>
    {
        c.Timeout = TimeSpan.FromSeconds(5);
    });;

builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

builder.Services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();
builder.Services.AddScoped<JwtService>();
builder.Services.Configure<AuthSettings>(builder.Configuration.GetSection("AuthSettings"));
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(
        builder.Configuration.GetConnectionString("Default")));

builder.Services.AddAuth(builder.Configuration); 
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAuthorization();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFront", policy =>
    {
        policy.WithOrigins("http://localhost:5173", "http://localhost:5174")
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});



var app = builder.Build();

app.UseCors("AllowFront");

app.UseAuthentication();
app.UseAuthorization();

app.UseSwagger();
app.UseSwaggerUI();
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<AppDbContext>();
        
        await context.Database.MigrateAsync();
        
        Console.WriteLine("База данных успешно обновлена и готова к работе.");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Ошибка при инициализации БД: {ex.Message}");
    }
}

app.Run();

