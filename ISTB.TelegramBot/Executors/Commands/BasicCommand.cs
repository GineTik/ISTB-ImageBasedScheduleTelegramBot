using ISTB.Framework.Attributes.TargetExecutorAttributes;
using ISTB.Framework.BotApplication.Storages.Interfaces;
using ISTB.Framework.Executors;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace ISTB.TelegramBot.Executors.Commands
{
    public class BasicCommand : Executor
    {
        private readonly ICommandStorage _commandStorage;

        public BasicCommand(ICommandStorage commandStorage)
        {
            _commandStorage = commandStorage;
        }

        [TargetCommands("start")]
        public async Task Start()
        {
            await SendTextAsync("Success");
            await UpdateContext.Client.SetMyCommandsAsync(_commandStorage.Commands);
            await UpdateContext.Client.GetMyCommandsAsync(BotCommandScope.Default());
        }
    }
}
