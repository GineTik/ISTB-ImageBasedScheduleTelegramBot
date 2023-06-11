using ISTB.BusinessLogic.DTOs.Schedule;
using ISTB.BusinessLogic.Services.Interfaces;
using ISTB.Framework.MessagePresets.Models;
using ISTB.Framework.TelegramBotApplication.Builders;
using ISTB.Framework.TelegramBotApplication.Context;
using ISTB.TelegramBot.Enum.Buttons;
using Telegram.Bot.Types.ReplyMarkups;

namespace ISTB.TelegramBot.MessagePresets.SchedulesMenu
{
    public class SchedulePresets
    {
        private readonly IScheduleService _service;
        private readonly UpdateContext _updateContext;

        public SchedulePresets(IScheduleService service, UpdateContextAccessor accessor)
        {
            _service = service;
            _updateContext = accessor.UpdateContext;
        }

        public async Task<MessagePreset> GetSchedulesAsync()
        {
            var schedules = await _service.GetListByTelegramUserIdAsync(_updateContext.TelegramUserId);

            var title = schedules.Count() switch
            {
                0 => "Ви ще не створили розклад",
                _ => "Ваші розклади"
            };

            var markup = new InlineKeyboardBuilder()
                .ButtonRange(
                    buttons: schedules.Select(s => InlineKeyboardButton.WithCallbackData(
                        s.Name, $"{nameof(ScheduleButtons.SelectSchedule)} {s.Id}")),
                    rowCount: 1
                ).Build();

            return new MessagePreset
            {
                Text = title,
                ReplyMarkup = markup
            };
        }

        public async Task<MessagePreset?> GetScheduleInfoAsync(int scheduleId)
        {
            var scheduleWithWeeks = await _service.GetWithWeeksByIdAsync(new GetScheduleByIdDTO()
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
                .ButtonRange(scheduleWithWeeks.Weeks.Select((week, i) => 
                    InlineKeyboardButton.WithCallbackData(
                        $"Тиждень {i + 1}", $"{nameof(ScheduleButtons.SelectScheduleWeek)} {week.Id}")),
                    rowCount: 2
                 )
                .CallbackButton("Створити тиждень", $"{nameof(ScheduleButtons.CreateScheduleWeek)} {scheduleWithWeeks.Id}")
                .CallbackButton("Видалити розклад", $"confirm_act {nameof(ScheduleButtons.RemoveSchedule)} | {scheduleWithWeeks.Id} {messageId}").EndRow()
                .CallbackButton("<<< Назад", nameof(ScheduleButtons.BackToMySchedules)).EndRow()
                .Build();

            return new MessagePreset
            {
                Text = $"Назва розкладу: {scheduleWithWeeks.Name}\nКількість розкладів: {scheduleWithWeeks.Weeks.Count()}",
                ReplyMarkup = markup
            };
        }

        public async Task<MessagePreset?> GetScheduleWeekInfoAsync(int weekId, int messageIdToEdit)
        {
            var week = await _service.GetWeekByIdAsync(weekId);

            if (week == null)
            {
                return null;
            }

            var markup = new InlineKeyboardBuilder()
                .CallbackButton("Видалити тиждень", $"confirm_act {nameof(ScheduleButtons.RemoveScheduleWeek)} | {weekId} {week.ScheduleId} {messageIdToEdit}").EndRow()
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
