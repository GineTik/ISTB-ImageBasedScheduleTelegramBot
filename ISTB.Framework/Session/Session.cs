using ISTB.Framework.Session.Storages.Interfaces;

namespace ISTB.Framework.Session
{
    public class Session<T>
    {
        private string _key => "SessionData:" + typeof(T).Name;

        private readonly ISessionDataStorage _dataStorage;

        public Session(ISessionDataStorage dataStorage)
        {
            _dataStorage = dataStorage;
        }

        public async Task<T?> GetAsync()
        {
            return await _dataStorage.GetAsync<T>(_key);
        }

        public async Task SetAsync(T value)
        {
            await _dataStorage.SetAsync<T>(_key, value);
        }

        public async Task RemoveAsync()
        {
            await _dataStorage.RemoveAsync(_key);
        }
    }
}
