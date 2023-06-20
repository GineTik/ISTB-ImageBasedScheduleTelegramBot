using ISTB.Framework.Attributes.TargetExecutorAttributes;
using System.Reflection;
using Telegram.Bot.Types;
using ISTB.Framework.Executors.Helpers.Factories.Interfaces;
using ISTB.Framework.Executors.Routing.Storages.TargetMethod;

namespace ISTB.Framework.Executors.Storages.Command
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
