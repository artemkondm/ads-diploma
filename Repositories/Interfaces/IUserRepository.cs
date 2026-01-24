using Ads.Models;

namespace Ads.Repositories.Interfaces;

public interface IUserRepository
{
    Task<User?> GetByEmailAsync(string email);
    Task<User?> GetByIdAsync(int userId);
    Task<bool> ExistsByEmailAsync(string email);
    Task AddAsync(User user);
}