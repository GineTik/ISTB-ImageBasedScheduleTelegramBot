using ISTB.Framework.Attributes.ValidateExecutionAttributes;
using ISTB.Framework.Executors;

namespace ISTB.TelegramBot.Executors.Commands
{
    [Command("start")]
    public class StartCommand : Executor
    {
        public override Task ExecuteAsync()
        {
            Console.WriteLine("Success");
            return Task.CompletedTask;
        }
    }
}
