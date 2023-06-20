using ISTB.Framework.Attributes.ParametersParse;
using ISTB.Framework.Attributes.TargetExecutorAttributes;
using ISTB.Framework.Executors;
using ISTB.Framework.Executors.Storages.Command;
using ISTB.Framework.TelegramBotApplication.AdvancedBotClient.Extensions;
using ISTB.Framework.TelegramBotApplication.Builders;
using ISTB.TelegramBot.Enum.Buttons;
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
            await Client.SendTextMessageAsync("Success");
            await Client.SetMyCommandsAsync(_commandStorage.Commands);
        }
    }
}
