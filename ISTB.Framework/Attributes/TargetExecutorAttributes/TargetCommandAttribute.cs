using Telegram.Bot.Types;

namespace ISTB.Framework.Attributes.TargetExecutorAttributes
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class TargetCommandAttribute : TargetExecutorAttribute
    {
        public string Command { get; set; }

        public TargetCommandAttribute(string command)
        {
            Command = command;
        }

        public override bool IsTarget(Message message)
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
