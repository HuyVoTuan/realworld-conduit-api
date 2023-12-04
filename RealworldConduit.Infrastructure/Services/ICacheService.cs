namespace RealworldConduit.Infrastructure.Services
{
    public interface ICacheService
    {
        T GetItem<T>(string key);
        bool SetItem<T>(string key, T value, TimeSpan expirationTime);
        object RemoveItem(string key);
    }
}
