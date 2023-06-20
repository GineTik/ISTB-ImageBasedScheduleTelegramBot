namespace ISTB.Framework.Executors.Routing.Storages.UserState.Saver
{
    public interface IUserStateSaver
    {
        Task SaveAsync(long userId, string state);
        Task<string?> LoadAsync(long userId);
        Task RemoveAsync(long userId);
    }
}
