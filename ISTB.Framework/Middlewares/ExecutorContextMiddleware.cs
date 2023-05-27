using ISTB.Framework.Executors.Context;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace ISTB.Framework.Middlewares
{
    public class ExecutorContextMiddleware : IMiddleware
    {
        private ExecutorContextAccessor _accessor;

        public ExecutorContextMiddleware(ExecutorContextAccessor accessor)
        {
            _accessor = accessor;
        }

        public async Task InvokeAsync(ITelegramBotClient botClient, Update update, Func<Task> next)
        {
            _accessor.ExecutorContext = new ExecutorContext
            {
                BotClient = botClient,
                Update = update,
            };
            
            await next();
        }
    }
}
