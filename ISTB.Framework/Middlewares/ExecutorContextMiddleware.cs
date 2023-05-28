using ISTB.Framework.Executors.Context;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace ISTB.Framework.Middlewares
{
    public class ExecutorContextMiddleware : IMiddleware
    {
        private UpdateContextAccessor _accessor;

        public ExecutorContextMiddleware(UpdateContextAccessor accessor)
        {
            _accessor = accessor;
        }

        public async Task InvokeAsync(ITelegramBotClient botClient, Update update, Func<Task> next)
        {
            _accessor.UpdateContext = new UpdateContext
            {
                BotClient = botClient,
                Update = update,
            };
            
            await next();
        }
    }
}
