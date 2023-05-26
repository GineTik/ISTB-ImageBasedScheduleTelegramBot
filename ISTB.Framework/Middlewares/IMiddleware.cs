using Telegram.Bot.Types;
using Telegram.Bot;

namespace ISTB.Framework.Middlewares
{
    public interface IMiddleware
    {
        public Task InvokeAsync(ITelegramBotClient botClient, Update update, Func<Task> next);
    }
}
