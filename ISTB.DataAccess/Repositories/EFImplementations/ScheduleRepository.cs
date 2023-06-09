using ISTB.DataAccess.EF;
using ISTB.DataAccess.Entities;
using ISTB.DataAccess.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ISTB.DataAccess.Repositories.EFImplementations
{
    public class ScheduleRepository : EFRepository<Schedule>, IScheduleRepository
    {
        public ScheduleRepository(DataContext context) : base(context)
        {
        }

        public async Task<Schedule?> GetByNameAsync(string name, long telegramUserId)
        {
            return await _context.Schedule
                .Include(schedule => schedule.User)
                .FirstOrDefaultAsync(schedule => schedule.Name == name && schedule.User.TelegramUserId == telegramUserId);
        }

        public async Task<ICollection<Schedule>> GetListByTelegramUserIdAsync(long telegramUserId)
        {
            return await _context.Schedule
                .Include(schedule => schedule.User)
                .Where(schedule => schedule.User.TelegramUserId == telegramUserId)
                .ToListAsync();
        }

        public async Task RemoveByNameAsync(string name, long telegramUserId)
        {
            var schedule = await GetByNameAsync(name, telegramUserId) ?? 
                throw new ArgumentException($"Name({name}) or TelegramUserId({telegramUserId}) not correct");

            _context.Schedule.Remove(schedule);
            await _context.SaveChangesAsync();
        }

        public async Task ChangeNameAsync(string oldName, string newName, long telegramUserId)
        {
            var schedule = await GetByNameAsync(oldName, telegramUserId) ??
                 throw new ArgumentException($"Name({oldName}) or TelegramUserId({telegramUserId}) not correct");

            schedule.Name = newName;
            _context.Schedule.Update(schedule);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> ScheduleByIdBelongsToUserAsync(int id, long telegramUserId)
        {
            return await _context.Schedule
                .Include(schedule => schedule.User)
                .AnyAsync(schedule => schedule.Id == id && schedule.User.TelegramUserId == telegramUserId);
        }
    }
}
