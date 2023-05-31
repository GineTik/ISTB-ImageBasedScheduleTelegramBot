using ISTB.Framework.BotApplication;
using ISTB.Framework.Middlewares;

namespace ISTB.Framework.Extensions.Middlewares
{
    public static class CommandExtension
    {
        public static BotApplication.BotApplication UseExecutorCommands(this BotApplication.BotApplication app)
        {
            return app.UseMiddleware<ExecutorCommandMiddleware>();
        }
    }
}
