namespace ISTB.Framework.Session.Storages.SessionSaver.Interfaces
{
    public interface ISessionDataSaver
    {
        Task SaveAsync<T>(long userId, string key, T data);
        Task<T?> LoadAsync<T>(long userId, string key);
        Task RemoveAsync(long userId, string key);
    }
}
