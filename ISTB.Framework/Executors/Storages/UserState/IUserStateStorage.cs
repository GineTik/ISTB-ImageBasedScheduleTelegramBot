namespace ISTB.Framework.Executors.Storages.UserState
{
    public interface IUserStateStorage
    {
        Task<string> GetAsync(long? telegramUserId = null);
        Task SetAsync(string status, long? telegramUserId = null);
        Task RemoveAsync(long? telegramUserId = null);
    }
}
