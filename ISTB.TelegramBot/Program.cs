using ISTB.BusinessLogic.AutoMapper.Profiles;
using ISTB.BusinessLogic.Services.Interfaces;
using ISTB.Framework.Executors.Configuration.Middleware.TargetExecutor;
using ISTB.Framework.Executors.Configuration.Options;
using ISTB.Framework.Executors.Configuration.Services;
using ISTB.Framework.Executors.Storages.UserState;
using ISTB.Framework.Session.Extensions.Services;
using ISTB.Framework.TelegramBotApplication;
using ISTB.Framework.TelegramBotApplication.Configuration.ReceiverOption;
using ISTB.TelegramBot.Extensions.Middlewares;
using ISTB.TelegramBot.Extensions.Services;
using Microsoft.Extensions.DependencyInjection;
using Telegram.Bot.Types.Enums;

namespace ISTB.TelegramBot
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            var builder = new BotApplicationBuilder();
            
            builder.ReceiverOptions.ConfigureAllowedUpdates(UpdateType.Message, UpdateType.CallbackQuery);

            builder.Services.AddData(builder.Configuration);
            builder.Services.AddAutoMapper(typeof(ScheduleProfile));
            builder.Services.AddServices();

            builder.Services.AddExecutors();
            builder.Services.AddSessions();

            var app = builder.Build();

            //app.Use(async (provider, updateContext, next) => {
            //    var userService = provider.GetRequiredService<IUserService>();
            //    var userStateStorage = provider.GetRequiredService<IUserStateStorage>();
            //    var userStateOptions = provider.GetRequiredService<UserStateOptions>();

            //    var role = await userService.GetRoleByTelegramUserIdAsync(updateContext.TelegramUserId);

            //    if (role != null &&
            //        await userStateStorage.GetAsync() == userStateOptions.DefaultUserState)
            //    {
            //        await userStateStorage.SetAsync(role);
            //    }

            //    await next();
            //});

            app.UseMyCatchException();
            app.UseExecutors();
            app.UseUpdateNotHandle();
            
            app.Run();

            Console.ReadLine();
        }
    }
}
