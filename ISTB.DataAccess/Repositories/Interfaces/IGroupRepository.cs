using ISTB.DataAccess.Entities;

namespace ISTB.DataAccess.Repositories.Interfaces
{
    public interface IGroupRepository : IRepository<Group>
    {
        Task<ICollection<Group>> GetListByTelegramUserIdAsync(long telegramUserId);
        Task<Group?> GetByNameAsync(string name, long telegramUserId);
        Task RemoveByNameAsync(string name, long telegramUserId);
        Task ChangeGroupNameAsync(string oldName, string newName, long telegramUserId);
    }
}
