using Telegram.Bot.Types;

namespace ISTB.Framework.Executors.Storages.Command
{
    public interface ICommandStorage
    {
        public IEnumerable<BotCommand> Commands { get; }
    }
}
