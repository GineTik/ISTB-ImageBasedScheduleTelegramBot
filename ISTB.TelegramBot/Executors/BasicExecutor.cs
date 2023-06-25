using ISTB.Framework.Attributes.TargetExecutorAttributes;
using ISTB.Framework.Executors;
using ISTB.Framework.Executors.Storages.Command;
using ISTB.Framework.TelegramBotApplication.AdvancedBotClient.Extensions;
using Telegram.Bot;

namespace ISTB.TelegramBot.Executors
{
    public sealed class BasicExecutor : Executor
    {
        private readonly ICommandStorage _commandStorage;

        public BasicExecutor(ICommandStorage commandStorage)
        {
            _commandStorage = commandStorage;
        }

        [TargetCommands("start")]
        public async Task StartCommand()
        {
            await Client.SendTextMessageAsync("Success");
            await Client.SetMyCommandsAsync(_commandStorage.Commands);
        }
    }
}
