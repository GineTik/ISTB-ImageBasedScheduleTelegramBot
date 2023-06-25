using ISTB.Framework.Attributes.ParametersParse;
using ISTB.Framework.Attributes.TargetExecutorAttributes;
using ISTB.Framework.Executors;
using ISTB.Framework.TelegramBotApplication.Builders;
using ISTB.Framework.TelegramBotApplication.Extensions.AdvancedTelegramBotClient;
using Telegram.Bot;

namespace ISTB.TelegramBot.Executors
{
    public class HelpExecutor : Executor
    {
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
