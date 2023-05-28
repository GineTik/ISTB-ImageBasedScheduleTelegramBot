using ISTB.Framework.Attributes.ValidateExecutionAttributes;
using ISTB.Framework.Executors;
using ISTB.Framework.Executors.Context;
using Telegram.Bot;

namespace ISTB.TelegramBot.Executors.Commands
{
    [Command("start")]
    public class StartCommand : Executor
    {
        private readonly UpdateContext _context;

        public StartCommand(UpdateContextAccessor accessor)
        {
            _context = accessor.UpdateContext;
        }

        public override async Task ExecuteAsync()
        {
            await _context.BotClient.SendTextMessageAsync(_context.ChatId, "Success");
        }
    }
}
