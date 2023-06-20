using ISTB.BusinessLogic.AutoMapper.Profiles;
using ISTB.Framework.Executors.Configuration.Middleware.TargetExecutor;
using ISTB.Framework.Executors.Configuration.Services;
using ISTB.Framework.Session.Extensions.Services;
using ISTB.Framework.TelegramBotApplication;
using ISTB.Framework.TelegramBotApplication.AdvancedBotClient.Extensions;
using ISTB.Framework.TelegramBotApplication.Configuration.Middlewares.CatchException;
using ISTB.Framework.TelegramBotApplication.Configuration.ReceiverOption;
using ISTB.TelegramBot.Extensions.Services;
using ISTB.TelegramBot.MessagePresets.SchedulesMenu;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
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
            builder.Services.AddTransient<SchedulePresets>();

            builder.Services.AddExecutors();
            builder.Services.AddSessions();

            var app = builder.Build();
            app.UseCatchException(async (updateContext, exception) =>
            {
                var message = exception switch
                {
                    TargetParameterCountException => "Ви забули ввести деякі параметри",
                    _ => exception.Message
                };
                await updateContext.Client.SendTextMessageAsync(message);
                Console.WriteLine(exception.ToString());
            });

            app.UseExecutors();

            app.Use(async (updateContext, _) => 
                await updateContext.Client.SendTextMessageAsync("Мені нема чим тобі відповіти")
            ); 
            app.Run();

            Console.ReadLine();
        }
    }
}
