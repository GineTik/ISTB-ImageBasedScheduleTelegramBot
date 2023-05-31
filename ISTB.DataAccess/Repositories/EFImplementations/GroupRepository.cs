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

        public ICollection<Group> GetListByTelegramUserId(long telegramUserId)
        {
            return _context.Groups
                .Include(group => group.User)
                .Where(group => group.User.TelegramUserId == telegramUserId)
                .ToList();
        }
    }
}
