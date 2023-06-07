using ISTB.Framework.Attributes.BaseAttributes;
using Telegram.Bot.Types;

namespace ISTB.Framework.Attributes.TargetExecutorAttributes
{
    public class TargetCommandsAttribute : TargetExecutorAttribute
    {
        public string[] Commands { get; set; }
        public string Description { get; set; }

        public TargetCommandsAttribute(string commands)
        {
            Commands = commands.Replace(" ", "").Split(',');
        }

        public override bool IsTarget(Update update)
        {
            if (update?.Message?.Text is not { } text)
                return false;

            var command = text.Split(' ').First();
            return Commands.Contains(command.TrimStart('/'));
        }
    }
}
