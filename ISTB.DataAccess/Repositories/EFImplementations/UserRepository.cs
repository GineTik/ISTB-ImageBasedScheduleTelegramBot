using ISTB.DataAccess.EF;
using ISTB.DataAccess.Entities;
using ISTB.DataAccess.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ISTB.DataAccess.Repositories.EFImplementations
{
    public class UserRepository : EFRepository<User>, IUserRepository
    {
        public UserRepository(DataContext context) : base(context)
        {
        }

        public async Task<User?> GetByTelegramUserIdAsync(long telegramUserId)
        {
            return await _context.Users.FirstOrDefaultAsync(user => user.TelegramUserId == telegramUserId);
        }
    }
}
