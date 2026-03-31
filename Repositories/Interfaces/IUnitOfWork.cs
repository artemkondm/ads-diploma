namespace Ads.Repositories.Interfaces;

public interface IUnitOfWork : IDisposable
{
    IRegionRepository Regions { get; }
    ICityRepository Cities { get; }
    ILocationRepository Locations { get; }
    IAdsRepository Ads { get; }
    IMessageRepository Messages { get; }
    IChatRepository Chats { get; }

    Task<int> SaveChangesAsync();
}