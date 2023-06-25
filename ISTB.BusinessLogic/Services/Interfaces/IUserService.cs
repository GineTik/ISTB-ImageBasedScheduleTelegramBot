namespace ISTB.BusinessLogic.Services.Interfaces
{
    public interface IUserService
    {
        public Task<string?> GetRoleByTelegramUserIdAsync(long telegramUserId);
    }
}
