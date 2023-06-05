using ISTB.Framework.Attributes.TargetExecutorAttributes;
using ISTB.Framework.Executors;

namespace ISTB.TelegramBot.Executors.Commands
{
    //[TargetCommands("start")]
    public class StartCommand : Executor
    {
        [TargetCommands("start")]
        public async Task ExecuteAsync()
        {
            await SendTextAsync("Success");
        }
    }
}
