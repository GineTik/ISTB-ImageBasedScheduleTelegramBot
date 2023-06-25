using ISTB.BusinessLogic.DTOs.Schedule;
using ISTB.Framework.Executors;
using ISTB.Framework.TelegramBotApplication.AdvancedBotClient.Extensions;
using ISTB.TelegramBot.Executors.Schedule;

namespace ISTB.TelegramBot.Views.Schedule
{
    public class RemoveScheduleView : Executor
    {
        public async Task ChooseScheduleToRemove(IEnumerable<ScheduleDTO> schedules)
        {
            await ExecuteAsync<ShowScheduleView>(v => v.ShowMySchedules(
                schedules,
                buttonsCallback: (s, _) => $"{nameof(RemoveScheduleExecutor.RemoveSchedule)} {s.Id}",
                hasUndoButton: true
            ));
        }

        public async Task ScheduleRemoved()
        {
            await Client.SendTextMessageAsync(
                "Розклад видалений"
            );
        }
    }
}
