using ISTB.Framework.Attributes.TargetExecutorAttributes;
using ISTB.Framework.BotApplication.Storages.Interfaces;
using ISTB.Framework.Executors;
using Telegram.Bot;

namespace ISTB.TelegramBot.Executors
{
    public class BasicExecutor : Executor
    {
        private readonly ICommandStorage _commandStorage;

        public BasicExecutor(ICommandStorage commandStorage)
        {
            _commandStorage = commandStorage;
        }

        [TargetCommands("start")]
        public async Task StartCommand()
        {
            await Client.SendTextResponseAsync("Success");
            await Client.SetMyCommandsAsync(_commandStorage.Commands);
        }
    }
}
