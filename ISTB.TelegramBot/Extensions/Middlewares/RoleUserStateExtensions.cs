using ISTB.BusinessLogic.Services.Interfaces;
using ISTB.Framework.Executors.Storages.UserState;
using ISTB.Framework.TelegramBotApplication;
using Microsoft.Extensions.DependencyInjection;

namespace ISTB.TelegramBot.Extensions.Middlewares
{
    public static class RoleUserStateExtensions
    {
        public static BotApplication UseRolesInUserStates(this BotApplication app)
        {
            return app.Use(async (provider, updateContext, next) =>
            {
                var userService = provider.GetRequiredService<IUserService>();
                var userStateStorage = provider.GetRequiredService<IUserStateStorage>();

                var role = await userService.GetRoleByTelegramUserIdAsync(updateContext.TelegramUserId);
                var states = await userStateStorage.GetAsync();

                if (role != null && states.Contains(role) == false)
                {
                    await userStateStorage.AddAsync(role);
                }

                await next();
            });
        }
    }
}
