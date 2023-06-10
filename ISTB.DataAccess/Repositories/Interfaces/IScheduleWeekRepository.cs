using ISTB.DataAccess.Entities;

namespace ISTB.DataAccess.Repositories.Interfaces
{
    public interface IScheduleWeekRepository : IRepository<ScheduleWeek>
    {
        Task<IEnumerable<ScheduleWeek>> GetByScheduleIdAsync(int scheduleId);
    }
}
