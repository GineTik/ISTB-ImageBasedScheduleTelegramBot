using ISTB.DataAccess.EF;
using ISTB.Framework.Attributes.ValidateExecutionAttributes;
using ISTB.Framework.Executors;
using ISTB.Framework.Executors.Context;
using Telegram.Bot;

namespace ISTB.TelegramBot.Executors.Commands
{
    [Command("start")]
    public class StartCommand : Executor
    {
        private readonly UpdateContext _userContext;

        public StartCommand(UpdateContextAccessor accessor, DataContext dbContext)
        {
            _userContext = accessor.UpdateContext;
        }

        public override async Task ExecuteAsync()
        {
            await _userContext.BotClient.SendTextMessageAsync(_userContext.ChatId, "Success");
        }
    }
}
