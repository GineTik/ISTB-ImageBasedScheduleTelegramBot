using ISTB.DataAccess.Entities;

namespace ISTB.DataAccess.Repositories.Interfaces
{
    public interface IScheduleRepository : IRepository<Schedule>
    {
        Task<ICollection<Schedule>> GetListByTelegramUserIdAsync(long telegramUserId);
        Task<Schedule?> GetByNameAsync(string name, long telegramUserId);
        Task RemoveByNameAsync(string name, long telegramUserId);
        Task ChangeNameAsync(int scheduleId, string newName);
        Task<bool> BelongsToUserAsync(int id, long telegramUserId);
    }
}
