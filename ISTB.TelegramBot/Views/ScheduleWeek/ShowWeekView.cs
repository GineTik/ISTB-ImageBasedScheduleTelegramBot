using ISTB.BusinessLogic.DTOs.ScheduleWeek;
using ISTB.Framework.Executors;
using ISTB.Framework.MessagePresets.Models;
using ISTB.Framework.TelegramBotApplication.AdvancedBotClient.Extensions;
using ISTB.Framework.TelegramBotApplication.Helpers.Builders;
using ISTB.TelegramBot.Executors.Schedule;
using ISTB.TelegramBot.Executors.ScheduleDay;
using ISTB.TelegramBot.Executors.ScheduleWeek;

namespace ISTB.TelegramBot.Views.ScheduleWeek
{
    public class ShowWeekView : Executor
    {
        public async Task ShowWeek(ScheduleWeekWithDaysDTO week)
        {
            var markup = new InlineKeyboardBuilder()
                .CallbackButtonList(
                    week.Days,
                    (day, _) => day.Name,
                    (day, _) => $"{nameof(GetDayExecutor.ShowDay)} {day.Position} {week.Id}",
                    rowCount: 2
                 )
                .CallbackButton("Видалити тиждень", $"confirm_act {nameof(RemoveWeekExecutor.RemoveWeek)} | {week.Id} {week.ScheduleId} {UpdateContext.MessageId}").EndRow()
                .CallbackButton($"Вибрати тиждень як поточний", $"{nameof(EditWeekExecutor.ChooseCurrentWeek)} {week.ScheduleId} {week.Position}").EndRow()
                .CallbackButton("<<< Назад", $"{nameof(GetScheduleExecutor.ShowSchedule)} {week.ScheduleId}").EndRow()
                .Build();

            await Client.EditMessageTextAsync(
                $"Тиждень {week.Id}",
                replyMarkup: markup
            );
        }
    }
}
