using ISTB.Framework.Executors.Middlewares;

namespace ISTB.Framework.Executors.Extensions.Middlewares
{
    public static class ExecutorExtension
    {
        public static BotApplication.BotApplication UseExecutors(this BotApplication.BotApplication app)
        {
            return app.UseMiddleware<TargetExecutorMiddleware>();
        }
    }
}
