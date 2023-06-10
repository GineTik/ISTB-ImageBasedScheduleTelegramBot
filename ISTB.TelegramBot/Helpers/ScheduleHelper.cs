using ISTB.BusinessLogic.DTOs.Schedule;
using ISTB.BusinessLogic.Services.Interfaces;
using ISTB.Framework.TelegramBotApplication.Context;
using ISTB.TelegramBot.Enum.Buttons;
using Telegram.Bot;
using Telegram.Bot.Types.ReplyMarkups;

namespace ISTB.TelegramBot.Helpers
{
    public static class ScheduleHelper
    {
        public static async Task<IEnumerable<IEnumerable<InlineKeyboardButton>>> GetMySchedulesAsButtonsAsync(IScheduleService service, long telegramUserId)
        {
            var schedules = await service.GetListByTelegramUserIdAsync(telegramUserId);

            var buttons = schedules.Select(schedule => new[] {
                InlineKeyboardButton.WithCallbackData(
                    schedule.Name,
                    $"{nameof(ScheduleButtons.SelectSchedule)} {schedule.Id}")
            });

            return buttons;
        }

        public static async Task EditScheduleInfoMessageAsync(IScheduleService service, 
            UpdateContext updateContext, int scheduleId)
        {
            var scheduleWithWeeks = await service.GetWithWeeksByIdAsync(new GetScheduleByIdDTO()
            {
                Id = scheduleId,
                TelegramUserId = updateContext.TelegramUserId,
            });

            if (scheduleWithWeeks == null)
                throw new ArgumentException(nameof(scheduleId));

            var markup = new InlineKeyboardMarkup(new[] {
                scheduleWithWeeks.Weeks.Select((week, i) => InlineKeyboardButton.WithCallbackData(
                    $"Тиждень {(week.Position == 0 ? i : week.Position)}", $"{nameof(ScheduleButtons.SelectScheduleWeek)} {week.Id}")),
                new[] { InlineKeyboardButton.WithCallbackData(
                    "Створити тиждень", $"{nameof(ScheduleButtons.CreateScheduleWeek)} {scheduleWithWeeks.Id}") },
                new[] { InlineKeyboardButton.WithCallbackData(
                    "Видалити розклад", $"{nameof(ScheduleButtons.RemoveSchedule)} {scheduleWithWeeks.Id}") },
                new[] { InlineKeyboardButton.WithCallbackData(
                    "<<< Назад", nameof(ScheduleButtons.BackToMySchedules)) }
            });

            await updateContext.Client.EditMessageTextAsync(
                updateContext.ChatId,
                updateContext.Update.CallbackQuery!.Message!.MessageId,
                $"Назва розкладу: {scheduleWithWeeks.Name}\nКількість розкладів: {scheduleWithWeeks.Weeks.Count()}",
                replyMarkup: markup
            );
        }

        public static async Task EditYourSchedulesMessageAsync(IScheduleService service,
            UpdateContext updateContext)
        {
            var buttons = await GetMySchedulesAsButtonsAsync(service, updateContext.TelegramUserId);
            await updateContext.Client.EditMessageTextAsync(
                updateContext.ChatId,
                updateContext.Update.CallbackQuery!.Message!.MessageId,
                "Ваші розклади",
                replyMarkup: new InlineKeyboardMarkup(buttons)
            );
        }
    }
}
