using ISTB.DataAccess.EF;
using ISTB.DataAccess.Entities;
using ISTB.DataAccess.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ISTB.DataAccess.Repositories.EFImplementations
{
    public class ScheduleDayRepository : EFRepository<ScheduleDay>, IScheduleDayRepository
    {
        public ScheduleDayRepository(DataContext context) : base(context)
        {
        }

        public async Task<ScheduleDay?> GetByDayNumber(int dayNumber, int weekId)
        {
            return await _context.SchedulesDays
                .FirstOrDefaultAsync(day => day.Position == dayNumber && day.ScheduleWeekId == weekId);
        }

        public async Task<IEnumerable<ScheduleDay>> GetByWeekIdAsync(int weekId)
        {
            return await _context.SchedulesDays
                .Where(day => day.ScheduleWeekId == weekId)
                .ToListAsync();
        }
    }
}
