using ISTB.DataAccess.EF;
using ISTB.DataAccess.Entities;
using ISTB.DataAccess.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ISTB.DataAccess.Repositories.EFImplementations
{
    public class ScheduleWeekRepository : EFRepository<ScheduleWeek>, IScheduleWeekRepository
    {
        public ScheduleWeekRepository(DataContext context) : base(context)
        {
        }

        public async Task<ScheduleWeek?> GetByPositionAsync(int scheduleId, uint position)
        {
            return await _context.SchedulesWeeks
                .FirstOrDefaultAsync(week => week.ScheduleId == scheduleId && week.Position == position);
        }

        public async Task<IEnumerable<ScheduleWeek>> GetByScheduleIdAsync(int scheduleId)
        {
            return await _context.SchedulesWeeks
                .Where(week => week.ScheduleId == scheduleId)
                .ToListAsync();
        }

        public async Task<int> GetWeeksCountByScheduleIdAsync(int scheduleId)
        {
            return await _context.SchedulesWeeks
                .Where(week => week.ScheduleId == scheduleId)
                .CountAsync();
        }
    }
}
