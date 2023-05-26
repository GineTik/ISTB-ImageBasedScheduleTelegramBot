using ISTB.Framework.BotConfigurations;
using ISTB.Framework.Middlewares;

namespace ISTB.Framework.Extensions.Middlewares
{
    public static class CommandExtension
    {
        public static BotApplication UseCommand(this BotApplication app)
        {
            return app.UseMiddleware<CommandMiddleware>();
        }
    }
}
