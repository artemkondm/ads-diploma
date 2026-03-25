using Ads.Models;

namespace Ads.Database;
using Microsoft.EntityFrameworkCore;
public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
    public DbSet<User> Users => Set<User>();
    public DbSet<Ad> Ads => Set<Ad>();
    public DbSet<Location> Locations => Set<Location>();
    public DbSet<City> Cities => Set<City>();
    public DbSet<Region> Regions => Set<Region>();
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
    }
}