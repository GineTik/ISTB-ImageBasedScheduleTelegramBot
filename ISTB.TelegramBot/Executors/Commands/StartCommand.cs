using ISTB.Framework.Attributes.TargetExecutorAttributes;
using ISTB.Framework.Executors;
using Telegram.Bot;

namespace ISTB.TelegramBot.Executors.Commands
{
    [TargetCommand("start")]
    public class StartCommand : Executor
    {
        public override async Task ExecuteAsync()
        {
            await SendTextAsync("Success");
        }
    }
}
