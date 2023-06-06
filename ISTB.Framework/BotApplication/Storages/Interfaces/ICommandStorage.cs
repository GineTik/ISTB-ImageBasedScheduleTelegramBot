using Telegram.Bot.Types;

namespace ISTB.Framework.BotApplication.Storages.Interfaces
{
    public interface ICommandStorage
    {
        public IEnumerable<BotCommand> Commands { get; }
    }
}
