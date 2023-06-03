using Telegram.Bot.Types;

namespace ISTB.Framework.Attributes.TargetExecutorAttributes
{
    public class TargetCommandsAttribute : TargetExecutorAttribute
    {
        public string[] Commands { get; set; }

        public TargetCommandsAttribute(string commands)
        {
            Commands = commands.Replace(" ", "").Split(",");
        }

        public override bool IsTarget(Message message)
        {
            if (message.Text is not { } text)
                return false;

            var command = text.Split(' ').FirstOrDefault();
            if (Commands.Any(c => '/' + c == command))
                return true;

            return false;
        }
    }
}
