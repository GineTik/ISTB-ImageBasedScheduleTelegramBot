using ISTB.Framework.Attributes.TargetExecutorAttributes;
using ISTB.Framework.BotApplication.Storages.Interfaces;
using ISTB.Framework.CreationalClasses.Factories.Interfaces;
using ISTB.Framework.Executors.Storages.Interfaces;
using System.Reflection;
using Telegram.Bot.Types;

namespace ISTB.Framework.Executors.Storages.Implementations
{
    public class ExecutorCommandStorage : ICommandStorage
    {
        public IEnumerable<BotCommand> Commands { get; }

        public ExecutorCommandStorage(ITargetMethodStorage storage, IBotCommandFactory factory)
        {
            Commands = storage.Methods
                .SelectMany(method => method
                    .GetCustomAttributes<TargetCommandsAttribute>()
                    .Select(attr => factory.CreateBotCommands(method, attr)))
                .SelectMany(commands => commands);
        }
    }
}
