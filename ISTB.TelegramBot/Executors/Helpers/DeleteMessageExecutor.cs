using ISTB.Framework.Attributes.TargetExecutorAttributes;
using ISTB.Framework.Executors;
using ISTB.Framework.TelegramBotApplication.AdvancedBotClient.Extensions;
using Telegram.Bot;

namespace ISTB.TelegramBot.Executors.Helpers
{
    public class DeleteMessageExecutor : Executor
    {
        [TargetCallbacksDatas(nameof(DeleteMessage))]
        public async Task DeleteMessage(int? messageIdToDeleted)
        {
            await Client.DeleteMessageAsync();

            if (messageIdToDeleted != null)
            {
                await Client.DeleteMessageAsync(
                    messageIdToDeleted.Value
                );
            }
        }
    }
}
