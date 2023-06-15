using ISTB.DataAccess.Entities;

namespace ISTB.DataAccess.Repositories.Interfaces
{
    public interface IScheduleDayRepository : IRepository<ScheduleDay>
    {
        Task<IEnumerable<ScheduleDay>> GetByWeekIdAsync(int weekId);
        Task<ScheduleDay?> GetByDayNumber(int dayNumber, int weekId);
    }
}
