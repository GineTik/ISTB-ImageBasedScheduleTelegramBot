using ISTB.DataAccess.EF;
using ISTB.DataAccess.Entities;
using ISTB.DataAccess.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ISTB.DataAccess.Repositories.EFImplementations
{
    public class GroupRepository : EFRepository<Group>, IGroupRepository
    {
        public GroupRepository(DataContext context) : base(context)
        {
        }

        public async Task<Group?> GetByNameAsync(string name, long telegramUserId)
        {
            return await _context.Groups
                .Include(group => group.User)
                .FirstOrDefaultAsync(group => group.Name == name && group.User.TelegramUserId == telegramUserId);
        }

        public async Task<ICollection<Group>> GetListByTelegramUserIdAsync(long telegramUserId)
        {
            return await _context.Groups
                .Include(group => group.User)
                .Where(group => group.User.TelegramUserId == telegramUserId)
                .ToListAsync();
        }

        public async Task RemoveByNameAsync(string name, long telegramUserId)
        {
            var group = await GetByNameAsync(name, telegramUserId) ?? 
                throw new ArgumentException($"Name({name}) or TelegramUserId({telegramUserId}) not correct");

            _context.Groups.Remove(group);
            await _context.SaveChangesAsync();
        }

        public async Task ChangeGroupNameAsync(string oldName, string newName, long telegramUserId)
        {
            var group = await GetByNameAsync(oldName, telegramUserId) ??
                 throw new ArgumentException($"Name({oldName}) or TelegramUserId({telegramUserId}) not correct");

            group.Name = newName;
            _context.Groups.Update(group);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> GroupByIdBelongsToUserAsync(int id, long telegramUserId)
        {
            return await _context.Groups
                .Include(group => group.User)
                .AnyAsync(group => group.Id == id && group.User.TelegramUserId == telegramUserId);
        }
    }
}
