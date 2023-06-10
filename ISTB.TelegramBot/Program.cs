using ISTB.BusinessLogic.AutoMapper.Profiles;
using ISTB.Framework.TelegramBotApplication;
using ISTB.Framework.TelegramBotApplication.Extensions.Middlwares;
using ISTB.Framework.TelegramBotApplication.Extensions.ReceiverOption;
using ISTB.Framework.Executors.Extensions.Middlewares;
using ISTB.Framework.Executors.Extensions.Services;
using ISTB.TelegramBot.Extensions.Services;
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
            builder.Services.AddExecutors();
            builder.Services.AddData(builder.Configuration);
            builder.Services.AddAutoMapper(typeof(ScheduleProfile));
            builder.Services.AddServices();

            var app = builder.Build();
            app.UseCatchException(async (updateContext, exception) =>
            {
                var message = exception switch
                {
                    TargetParameterCountException => "Ви забули ввести деякі параметри",
                    _ => exception.Message
                };
                await updateContext.Client.SendTextResponseAsync(message);
            });
            app.UseExecutors();
            app.Use(async (UpdateContext, _) => 
                await UpdateContext.Client.SendTextResponseAsync("Мені нема чим тобі відповіти")
            ); 
            app.Run();

            Console.ReadLine();
        }
    }
}
