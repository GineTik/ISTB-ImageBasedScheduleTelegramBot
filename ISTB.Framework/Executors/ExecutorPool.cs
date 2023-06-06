using ISTB.Framework.Attributes.BaseAttributes;
using ISTB.Framework.Attributes.TargetExecutorAttributes;
using System.Reflection;
using Telegram.Bot.Types;

namespace ISTB.Framework.Executors
{
    public class ExecutorPool
    {
        public IEnumerable<Type> ExecutorsTypes { get; }
        public IEnumerable<MethodInfo> TargetMethods { get; }
        public IEnumerable<BotCommand> CommandsInfo { get; }

        public ExecutorPool(IEnumerable<Type> executorsTypes)
        {
            ExecutorsTypes = executorsTypes;

            TargetMethods = ExecutorsTypes
                .SelectMany(type => type.GetMethods())
                .Where(method => method.GetCustomAttributes<TargetExecutorAttribute>().Count() > 0);

            CommandsInfo = TargetMethods
                .SelectMany(method => method.GetCustomAttributes<TargetCommandsAttribute>())
                .Select(attr => attr.Commands.Select(command => new BotCommand { Command = command, Description = attr.Description }))
                .SelectMany(list => list);
        }

        public MethodInfo? GetMethodInfoByUpdate(Update update)
        {
            return TargetMethods.FirstOrDefault(method => 
                method
                    .GetCustomAttributes<TargetExecutorAttribute>()
                    .Any(attr => attr.IsTarget(update))
            );
        }
    }
}
