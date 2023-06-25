using ISTB.DataAccess.Entities;

namespace ISTB.DataAccess.Repositories.Interfaces
{
    public interface IScheduleWeekRepository : IRepository<ScheduleWeek>
    {
        Task<IEnumerable<ScheduleWeek>> GetByScheduleIdAsync(int scheduleId);
        Task<ScheduleWeek?> GetByPositionAsync(int scheduleId, uint position);
        Task<int> GetWeeksCountByScheduleIdAsync(int scheduleId);
    }
}
