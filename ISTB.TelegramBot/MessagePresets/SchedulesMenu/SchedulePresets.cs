using ISTB.BusinessLogic.DTOs.Schedule;
using ISTB.BusinessLogic.Services.Interfaces;
using ISTB.Framework.MessagePresets.Models;
using ISTB.Framework.TelegramBotApplication.Builders;
using ISTB.Framework.TelegramBotApplication.Context;
using ISTB.TelegramBot.Enum.Buttons;

namespace ISTB.TelegramBot.MessagePresets.SchedulesMenu
{
    public class SchedulePresets
    {
        private readonly IScheduleService _scheduleService;
        private readonly IScheduleWeekService _weekService;
        private readonly UpdateContext _updateContext;

        public SchedulePresets(IScheduleService service, UpdateContextAccessor accessor, IScheduleWeekService weekService)
        {
            _scheduleService = service;
            _updateContext = accessor.UpdateContext;
            _weekService = weekService;
        }

        public async Task<MessagePreset> GetSchedulesAsync()
        {
            var schedules = await _scheduleService.GetListByTelegramUserIdAsync(_updateContext.TelegramUserId);

            var username = 
                _updateContext.Update.Message?.From?.Username ??
                _updateContext.Update.CallbackQuery?.Message?.Chat?.Username ??
                "";

            var title = schedules.Count() switch
            {
                0 => "Ви ще не створили розклад",
                _ => $"Розклади користувача @{username}"
            };

            var markup = new InlineKeyboardBuilder()
                .CallbackButtonList(
                    schedules,
                    (schedule, _) => schedule.Name, 
                    (schedule, _) => $"{nameof(ScheduleButtons.SelectSchedule)} {schedule.Id}",
                    rowCount: 2
                ).Build();

            return new MessagePreset
            {
                Text = title,
                ReplyMarkup = markup
            };
        }

        public async Task<MessagePreset?> GetScheduleInfoAsync(int scheduleId)
        {
            var scheduleWithWeeks = await _scheduleService.GetWithWeeksByIdAsync(new GetScheduleByIdDTO()
            {
                Id = scheduleId,
                TelegramUserId = _updateContext.TelegramUserId,
            });

            if (scheduleWithWeeks == null)
            {
                return null;
            }

            var messageId = _updateContext.Update.CallbackQuery?.Message?.MessageId
                ?? throw new InvalidOperationException("UpdateType is not CallbackQuery");

            var markup = new InlineKeyboardBuilder()
                .CallbackButtonList(
                    scheduleWithWeeks.Weeks, 
                    (_, i) => $"Тиждень {i + 1}", 
                    (week, _) => $"{nameof(WeekButtons.SelectScheduleWeek)} {week.Id}",
                    rowCount: 2
                 )
                .CallbackButton("Створити тиждень", $"{nameof(WeekButtons.CreateScheduleWeek)} {scheduleWithWeeks.Id}").EndRow()
                .CallbackButton("Налаштування", $"{nameof(ScheduleButtons.ScheduleSettings)} {scheduleWithWeeks.Id}").EndRow()
                .CallbackButton("<<< Назад", nameof(ScheduleButtons.SelectSchedules)).EndRow()
                .Build();

            return new MessagePreset
            {
                Text = $"Назва розкладу: {scheduleWithWeeks.Name}\nКількість розкладів: {scheduleWithWeeks.Weeks.Count()}",
                ReplyMarkup = markup
            };
        }

        public async Task<MessagePreset?> GetScheduleWeekInfoAsync(int weekId, int messageIdToEdit)
        {
            var week = await _weekService.GetWeekByIdAsync(weekId);

            if (week == null)
            {
                return null;
            }

            var markup = new InlineKeyboardBuilder()
                .CallbackButtonList(
                    week.Days,
                    (day, _) => day.Name,
                    (day, _) => $"{nameof(DayButtons.SelectScheduleDay)} {day.Position} {week.Id}",
                    rowCount: 2
                 )
                .CallbackButton("Видалити тиждень", $"confirm_act {nameof(WeekButtons.RemoveScheduleWeek)} | {weekId} {week.ScheduleId} {messageIdToEdit}").EndRow()
                .CallbackButton("<<< Назад", $"{nameof(ScheduleButtons.SelectSchedule)} {week.ScheduleId}").EndRow()
                .Build();

            return new MessagePreset
            {
                Text = $"Тиждень {week.Id}",
                ReplyMarkup = markup,
            };
        }
    }
}
