using ISTB.Framework.Attributes.TargetExecutorAttributes;
using ISTB.Framework.BotApplication.Storages.Interfaces;
using ISTB.Framework.Executors;
using Telegram.Bot;
using Telegram.Bot.Types.ReplyMarkups;

namespace ISTB.TelegramBot.Executors.Commands
{
    public class BasicCommands : Executor
    {
        private readonly ICommandStorage _commandStorage;

        public BasicCommands(ICommandStorage commandStorage)
        {
            _commandStorage = commandStorage;
        }

        [TargetCommands("start")]
        public async Task Start()
        {
            await UpdateContext.Client.SendTextMessageAsync(
                UpdateContext.ChatId, 
                "Success"
            );

            await UpdateContext.Client.SetMyCommandsAsync(_commandStorage.Commands);
        }
    }
}
