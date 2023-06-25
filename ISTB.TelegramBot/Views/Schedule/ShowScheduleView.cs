using ISTB.BusinessLogic.DTOs.Schedule;
using ISTB.Framework.Executors;
using ISTB.Framework.TelegramBotApplication.AdvancedBotClient.Extensions;
using ISTB.Framework.TelegramBotApplication.Context;
using ISTB.Framework.TelegramBotApplication.Helpers.Builders;
using ISTB.TelegramBot.Executors.Schedule;
using ISTB.TelegramBot.Executors.ScheduleWeek;
using Telegram.Bot;
using Telegram.Bot.Types.Enums;

namespace ISTB.TelegramBot.Views.Schedule
{
    public class ShowScheduleView : Executor
    {
        public async Task ShowMySchedules(IEnumerable<ScheduleDTO> schedules, 
            Func<ScheduleDTO, int, string>? buttonsCallback = null, bool hasUndoButton = false)
        {
            if (schedules.Count() == 0)
            {
                await Client.SendTextMessageAsync(
                   "Ви ще не створили розклад"
                );
                return;
            }

            var username = UpdateContext.User.ToString();
            var title = $"Розклади користувача {username}";

            var markupBuilder = new InlineKeyboardBuilder()
                .CallbackButtonList(
                    schedules,
                    (schedule, _) => schedule.Name,
                    buttonsCallback ?? ((schedule, _) => $"{nameof(GetScheduleExecutor.ShowSchedule)} {schedule.Id}"),
                    rowCount: 2
                );

            if (hasUndoButton == true)
            {
                markupBuilder.CallbackButton("Відмінити", $"delete_message {UpdateContext.MessageId}");
            }

            var markup = markupBuilder.Build();

            if (UpdateContext.Update.Type == UpdateType.Message)
            {
                await Client.SendTextMessageAsync(
                    title,
                    replyMarkup: markup
                );
            }
            else
            {
                await Client.EditMessageTextAsync(
                    title,
                    replyMarkup: markup
                );
            }
        }

        public async Task ShowSchedule(ScheduleWithWeeksDTO schedule)
        {
            var markup = new InlineKeyboardBuilder()
                .CallbackButtonList(
                    schedule.Weeks,
                    (week, _) => $"Тиждень {week.Position + 1}",
                    (week, _) => $"{nameof(GetWeekExecutor.ShowWeek)} {week.Id}",
                    rowCount: 2
                 )
                .CallbackButton("Створити тиждень", $"{nameof(CreateWeekExecutor.CreateWeek)} {schedule.Id} {schedule.Weeks.Count()}").EndRow()
                .CallbackButton("<<< Назад", nameof(GetScheduleExecutor.ShowMySchedules)).EndRow()
                .Build();

            await Client.EditMessageTextAsync(
                $"Назва розкладу: {schedule.Name}\n" +
                $"Кількість розкладів: {schedule.Weeks.Count()}\n" +
                $"Код розкладу: {schedule.Id}",
                replyMarkup: markup
            );
        }
    }
}
