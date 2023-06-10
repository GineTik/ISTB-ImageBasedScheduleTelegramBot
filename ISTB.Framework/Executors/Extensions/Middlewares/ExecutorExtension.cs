using ISTB.Framework.Executors.Middlewares;

namespace ISTB.Framework.Executors.Extensions.Middlewares
{
    public static class ExecutorExtension
    {
        public static TelegramBotApplication.BotApplication UseExecutors(this TelegramBotApplication.BotApplication app)
        {
            return app.UseMiddleware<TargetExecutorMiddleware>();
        }
    }
}
