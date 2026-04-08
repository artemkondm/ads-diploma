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
    public DbSet<Message> Messages => Set<Message>();
    public DbSet<Chat> Chats => Set<Chat>();
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<User>()
            .Property(u => u.Role)
            .HasConversion<string>();
        modelBuilder.Entity<User>()
            .Property(u => u.Status)
            .HasConversion<string>();
        
        
        modelBuilder.Entity<Ad>()
            .Property(a => a.Status)
            .HasConversion<string>();
        
        modelBuilder.Entity<Category>().HasData(
            new Category { Id = 1, Name = "Транспорт", ParentId = null },
            new Category { Id = 2, Name = "Легковые авто", ParentId = 1 },
            new Category { Id = 3, Name = "Мотоциклы", ParentId = 1 },
            new Category { Id = 4, Name = "Электроника", ParentId = null },
            new Category { Id = 5, Name = "Смартфоны", ParentId = 4 },
            new Category { Id = 6, Name = "Компьютеры", ParentId = 4 }
        );
    }
}