using ISTB.Framework.Middlewares;

namespace ISTB.Framework.Extensions.Middlewares
{
    public static class ExecutorExtension
    {
        public static BotApplication.BotApplication UseTargetExecutor(this BotApplication.BotApplication app)
        {
            return app.UseMiddleware<TargetExecutorMiddleware>();
        }
    }
}
