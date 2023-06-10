using ISTB.Framework.Attributes.TargetExecutorAttributes;
using System.Reflection;
using Telegram.Bot.Types;

namespace ISTB.Framework.Executors.Factories.Interfaces
{
    public interface IBotCommandFactory
    {
        IEnumerable<BotCommand> CreateBotCommands(MethodInfo method, TargetCommandsAttribute attribute);
    }
}
