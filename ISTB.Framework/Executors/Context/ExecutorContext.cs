using Telegram.Bot;
using Telegram.Bot.Types;

namespace ISTB.Framework.Executors.Context
{
    public class ExecutorContext
    {
        public ITelegramBotClient BotClient { get; set; }
        public Update Update { get; set; }
        public long ChatId => Update.Message.Chat.Id;
    }
}
