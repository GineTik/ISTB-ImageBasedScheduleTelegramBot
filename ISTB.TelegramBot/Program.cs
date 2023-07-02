using ISTB.BusinessLogic.AutoMapper.Profiles;
using ISTB.Framework.Executors.Configuration.Middleware.TargetExecutor;
using ISTB.Framework.Executors.Configuration.Services;
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

            app.UseMyCatchException();

            app.UseRolesInUserStates();
            app.UseExecutors();
            
            app.UseUpdateNotHandle();
            app.Run();

            Console.ReadLine();
        }
    }
}
