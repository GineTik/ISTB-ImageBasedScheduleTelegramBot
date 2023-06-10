using ISTB.Framework.Attributes.TargetExecutorAttributes;
using ISTB.Framework.Executors.Factories.Interfaces;
using System.Reflection;
using Telegram.Bot.Types;

namespace ISTB.Framework.Executors.Factories.Implementations
{
    public class ExecutorBotCommandFactory : IBotCommandFactory
    {
        public IEnumerable<BotCommand> CreateBotCommands(MethodInfo method, TargetCommandsAttribute attribute)
        {
            yield return new BotCommand
            {
                Command = getMainCommand(attribute.Commands),
                Description = $"{getSecondCommands(attribute.Commands)} <{getParameters(method)}> {attribute.Description}",
            };
        }

        private string getMainCommand(IEnumerable<string> commands)
        {
            return commands.First();
        }

        private string getSecondCommands(IEnumerable<string> commands)
        {
            if (commands.Count() > 1)
                return $"({string.Join(", ", commands.Skip(1).Select(c => "/" + c))})";
            return "";
        }

        private string getParameters(MethodInfo method)
        {
            return string.Join("> <", method.GetParameters().Select(param => param.Name));
        }
    }
}
