using ISTB.Framework.TelegramBotApplication;
using ISTB.Framework.TelegramBotApplication.AdvancedBotClient.Extensions;

namespace ISTB.TelegramBot.Extensions.Middlewares
{
    public static class UpdateNotHandleExtensions
    {
        public static BotApplication UseUpdateNotHandle(this BotApplication app)
        {
            return app.Use(async (updateContext, _) =>
               await updateContext.Client.SendTextMessageAsync("Мені нема чим тобі відповіти")
            );
        }
    }
}
