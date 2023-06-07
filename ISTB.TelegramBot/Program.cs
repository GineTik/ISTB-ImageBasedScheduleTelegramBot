using ISTB.BusinessLogic.AutoMapper.Profiles;
using ISTB.Framework.BotApplication;
using ISTB.Framework.BotApplication.Extensions.Middlwares;
using ISTB.Framework.BotApplication.Extensions.ReceiverOption;
using ISTB.Framework.Executors.Extensions.Middlewares;
using ISTB.Framework.Executors.Extensions.Services;
using ISTB.TelegramBot.Extensions.Services;
using Microsoft.Extensions.DependencyInjection;
using Telegram.Bot;
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
            builder.Services.AddAutoMapper(typeof(GroupProfile));
            builder.Services.AddServices();

            var app = builder.Build();
            app.UseCatchException(async (updateContext, exception) =>
            {
                await updateContext.Client.SendTextMessageAsync(
                    updateContext.ChatId, 
                    exception.Message);
            });
            app.UseTargetExecutor();
            app.Use(async (UpdateContext, _) => await UpdateContext.Client.SendTextMessageAsync(
                UpdateContext.ChatId, 
                "Мені нема чим тобі відповіти"));
            app.Run();

            Console.ReadLine();
        }
    }
}
