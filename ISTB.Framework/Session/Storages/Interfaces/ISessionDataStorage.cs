namespace ISTB.Framework.Session.Storages.Interfaces
{
    public interface ISessionDataStorage
    {
        public Task<T?> GetAsync<T>(string key);
        public Task SetAsync<T>(string key, T data);
        public Task RemoveAsync(string key);
    }
}
