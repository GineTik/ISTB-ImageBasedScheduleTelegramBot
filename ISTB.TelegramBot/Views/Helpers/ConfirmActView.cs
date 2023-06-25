using ISTB.Framework.Executors;
using ISTB.Framework.TelegramBotApplication.AdvancedBotClient.Extensions;
using ISTB.Framework.TelegramBotApplication.Helpers.Builders;

namespace ISTB.TelegramBot.Views.Helpers
{
    public class ConfirmActView : Executor
    {
        public async Task ShowConfirmAct(string targetCallbackData, string data)
        {
            await Client.SendTextMessageAsync(
            "Ви впевнені?",
                replyMarkup: new InlineKeyboardBuilder()
                    .CallbackButton("Так", $"{targetCallbackData} {data}")
                    .CallbackButton("Ні", "delete_message").EndRow()
                    .Build()
            );
        }
    }
}
