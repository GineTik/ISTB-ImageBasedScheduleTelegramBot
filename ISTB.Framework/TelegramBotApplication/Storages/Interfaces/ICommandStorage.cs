using Telegram.Bot.Types;

namespace ISTB.Framework.TelegramBotApplication.Storages.Interfaces
{
    public interface ICommandStorage
    {
        public IEnumerable<BotCommand> Commands { get; }
    }
}
