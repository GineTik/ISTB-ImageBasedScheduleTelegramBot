using ISTB.Framework.Attributes.TargetExecutorAttributes;
using ISTB.Framework.BotApplication.Storages.Interfaces;
using ISTB.Framework.Executors.Storages.Interfaces;
using System.Reflection;
using Telegram.Bot.Types;

namespace ISTB.Framework.Executors.Storages.Implementations
{
    public class ExecutorCommandStorage : ICommandStorage
    {
        public IEnumerable<BotCommand> Commands { get; }

        public ExecutorCommandStorage(ITargetMethodStorage storage)
        {
            Commands = storage.Methods
                .SelectMany(method => method.GetCustomAttributes<TargetCommandsAttribute>()
                    .Select(attr => attr.Commands.Select(command => new BotCommand 
                    { 
                        Command = command, 
                        Description = "<" + String.Join("> <", method.GetParameters().Select(param => param.Name)) + "> " + attr.Description,
                    }))
                    .SelectMany(list => list)
                );
        }
    }
}
