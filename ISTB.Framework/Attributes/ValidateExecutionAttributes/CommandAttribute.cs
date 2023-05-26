using ISTB.Framework.Executors;
using Telegram.Bot.Types;

namespace ISTB.Framework.Attributes.ValidateExecutionAttributes
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class CommandAttribute : ValidateExecutionAttribute
    {
        public string Command { get; set; }

        public CommandAttribute(string command)
        {
            Command = command;
        }

        public override bool ValidateExecution(Message message)
        {
            if (message.Text is not { } text)
                return false;

            var command = text.Split(' ').FirstOrDefault();
            if (command == '/' + Command)
                return true;

            return false;
        }
    }
}
