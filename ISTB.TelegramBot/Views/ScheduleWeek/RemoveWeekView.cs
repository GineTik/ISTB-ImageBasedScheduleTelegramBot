using ISTB.Framework.Executors;
using ISTB.Framework.TelegramBotApplication.AdvancedBotClient.Extensions;
using ISTB.TelegramBot.Executors.Schedule;

namespace ISTB.TelegramBot.Views.ScheduleWeek
{
    public class RemoveWeekView : Executor
    {
        public async Task WeekRemoved(int weekId, int scheduleId, int messageIdToEdit)
        {
            await Client.SendTextMessageAsync($"week id to remove: {weekId}, messageId: {messageIdToEdit}");
            await ExecuteAsync<GetScheduleExecutor>(e => e.ShowSchedule(scheduleId));
        }
    }
}
