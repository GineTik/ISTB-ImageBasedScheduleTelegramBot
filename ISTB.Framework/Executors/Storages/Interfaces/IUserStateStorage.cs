namespace ISTB.Framework.Executors.Storages.Interfaces
{
    public interface IUserStateStorage
    {
        Task<string> GetAsync(long? telegramUserId = null);
        Task SetAsync(string status);
        Task SetAsync(long telegramUserId, string status);
        Task RemoveAsync(long? telegramUserId = null);
    }
}
