using ISTB.Framework.Attributes.ParametersParse;
using ISTB.Framework.Attributes.TargetExecutorAttributes;
using ISTB.Framework.Executors;
using ISTB.Framework.TelegramBotApplication.Builders;
using ISTB.Framework.TelegramBotApplication.Extensions.AdvancedTelegramBotClient;
using ISTB.Framework.TelegramBotApplication.Storages.Interfaces;
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

        [TargetCallbacksDatas("confirm_act")]
        [ParametersSeparator(" | ")]
        public async Task ConfirmYourAct(string targetCallbackData, string data)
        {
            await Client.AnswerCallbackQueryAsync();
            await Client.SendTextMessageAsync(
                "Ви впевнені?",
                replyMarkup: new InlineKeyboardBuilder()
                    .CallbackButton("Так", $"{targetCallbackData} {data}")
                    .CallbackButton("Ні", "delete_message").EndRow()
                    .Build()
            );
        }

        [TargetCallbacksDatas("delete_message")]
        public async Task DeleteMessage(int? messageIdToDeleted)
        {
            await Client.DeleteCallbackQueryMessageAsync();

            if (messageIdToDeleted != null)
            {
                await Client.DeleteMessageAsync(
                    UpdateContext.ChatId,
                    messageIdToDeleted.Value
                );
            }
        }
    }
}
