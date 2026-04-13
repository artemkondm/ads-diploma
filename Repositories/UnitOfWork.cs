using Ads.Database;
using Ads.Repositories.Interfaces;

namespace Ads.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly AppDbContext _context;
    
    public IRegionRepository Regions { get; }
    public ICityRepository Cities { get; }
    public ILocationRepository Locations { get; }
    public IAdsRepository Ads { get; }
    public IMessageRepository Messages { get; }
    public IChatRepository Chats { get; }
    public IFavoritesRepository Favorites { get; }

    public UnitOfWork(AppDbContext context)
    {
        _context = context;
        Regions = new RegionRepository(_context);
        Cities = new CityRepository(_context);
        Locations = new LocationRepository(_context);
        Ads = new AdsRepository(_context);
        Messages = new MessageRepository(_context);
        Chats = new ChatRepository(_context);
        Favorites = new FavoritesRepository(_context);
    }

    public async Task<int> SaveChangesAsync()
    {
        return await _context.SaveChangesAsync();
    }
    
    public void Dispose()
    {
        _context.Dispose();
    }
}