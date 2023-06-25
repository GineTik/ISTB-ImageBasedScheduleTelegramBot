using ISTB.Framework.TelegramBotApplication;
using ISTB.Framework.TelegramBotApplication.AdvancedBotClient.Extensions;
using ISTB.Framework.TelegramBotApplication.Configuration.Middlewares.CatchException;
using System.Reflection;

namespace ISTB.TelegramBot.Extensions.Middlewares
{
    public static class CatchExceptionExtensions
    {
        public static BotApplication UseMyCatchException(this BotApplication app)
        {
            return app.UseCatchException(async (updateContext, exception) =>
            {
                var message = exception switch
                {
                    TargetParameterCountException => "Ви забули ввести деякі параметри",
                    _ => exception.Message
                };
                await updateContext.Client.SendTextMessageAsync(message);
                Console.WriteLine(exception.ToString());
            });
        }
    }
}
