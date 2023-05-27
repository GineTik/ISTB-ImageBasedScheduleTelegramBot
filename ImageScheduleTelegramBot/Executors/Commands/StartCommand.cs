using ISTB.Framework.Attributes.ValidateExecutionAttributes;
using ISTB.Framework.Executors;
using ISTB.Framework.Executors.Context;
using Telegram.Bot;

namespace ISTB.TelegramBot.Executors.Commands
{
    [Command("start")]
    public class StartCommand : Executor
    {
        private readonly ExecutorContext _context;

        public StartCommand(ExecutorContextAccessor accessor)
        {
            _context = accessor.ExecutorContext;
        }

        public override async Task ExecuteAsync()
        {
            await _context.BotClient.SendTextMessageAsync(_context.ChatId, "Success");
        }
    }
}
