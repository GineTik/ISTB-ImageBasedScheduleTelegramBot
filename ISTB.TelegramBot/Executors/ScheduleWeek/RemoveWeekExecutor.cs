using ISTB.Framework.Attributes.TargetExecutorAttributes;
using ISTB.Framework.Executors;
using ISTB.Framework.TelegramBotApplication.AdvancedBotClient.Extensions;
using ISTB.TelegramBot.Views.ScheduleWeek;

namespace ISTB.TelegramBot.Executors.ScheduleWeek
{
    public class RemoveWeekExecutor : Executor
    {
        [TargetCallbacksDatas(nameof(RemoveWeek))]
        public async Task RemoveWeek(int weekId, int scheduleId, int messageIdToEdit)
        {
            await Client.AnswerCallbackQueryAsync();
            await Client.DeleteMessageAsync();

            await ExecuteAsync<RemoveWeekView>(v => v.WeekRemoved(weekId, scheduleId, messageIdToEdit));
        }
    }
}
