namespace ISTB.Framework.Executors.Storages.UserStateSaver.Interfaces
{
    public interface IUserStateSaver
    {
        Task SaveAsync(long userId, string state);
        Task<string?> LoadAsync(long userId);
        Task RemoveAsync(long userId);
    }
}
