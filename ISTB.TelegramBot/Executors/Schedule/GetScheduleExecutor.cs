using ISTB.BusinessLogic.Services.Interfaces;
using ISTB.Framework.Attributes.TargetExecutorAttributes;
using ISTB.Framework.Executors;
using ISTB.Framework.MessagePresets.Extensions.AdvancedTelegramBotClient;
using ISTB.Framework.TelegramBotApplication.Builders;
using ISTB.Framework.TelegramBotApplication.Extensions.AdvancedTelegramBotClient;
using ISTB.TelegramBot.Enum.Buttons;
using ISTB.TelegramBot.MessagePresets.SchedulesMenu;
using Telegram.Bot;
using Telegram.Bot.Types.Enums;

namespace ISTB.TelegramBot.Executors.Schedule
{
    public class GetScheduleExecutor : Executor
    {
        private readonly IScheduleService _service;
        private readonly SchedulePresets _presets;

        public GetScheduleExecutor(IScheduleService service, SchedulePresets presets)
        {
            _service = service;
            _presets = presets;
        }

        [TargetCommands("schedules")]
        [TargetCallbacksDatas(nameof(ScheduleButtons.SelectSchedules))]
        public async Task GetMySchedule()
        {
            var preset = await _presets.GetSchedulesAsync();

            if (UpdateContext.Update.Type == UpdateType.Message)
            {
                await Client.SendMessageResponseAsync(preset);
            }
            else
            {
                var messageId = UpdateContext.MessageId;
                await Client.EditMessageResponseAsync(messageId, preset);
            }
        }

        [TargetCallbacksDatas(nameof(ScheduleButtons.SelectSchedule))]
        public async Task SelectSchedule(int scheduleId)
        {
            await Client.AnswerCallbackQueryAsync();
            var preset = 
                await _presets.GetScheduleInfoAsync(scheduleId) ??
                await _presets.GetSchedulesAsync();

            await Client.EditMessageResponseAsync(
                UpdateContext.Update.CallbackQuery!.Message!.MessageId,
                preset
            );
        }

        [TargetCallbacksDatas(nameof(ScheduleButtons.ScheduleSettings))]
        public async Task ScheduleSettings(int scheduleId)
        {
            await Client.AnswerCallbackQueryAsync();
           
            var messageId = UpdateContext.MessageId;
            var markup = new InlineKeyboardBuilder()
                //.CallbackButton("Видалити розклад", $"confirm_act {nameof(ScheduleButtons.RemoveSchedule)} | {scheduleId} {messageId}").EndRow()
                .CallbackButton("<<< Назад", $"{nameof(ScheduleButtons.SelectSchedule)} {scheduleId}").EndRow()
                .Build();
            
            await Client.EditMessageReplyMarkupAsync(
                UpdateContext.ChatId,
                messageId,
                replyMarkup: markup
            );
        }

        public async Task SendSchedules(string text, ScheduleButtons button)
        {
            var schedules = await _service.GetListByTelegramUserIdAsync(UpdateContext.TelegramUserId);

            if (schedules.Count == 0)
            {
                await Client.SendTextMessageAsync(
                    "Ви ще не створили розклад"
                );
            }
            else
            {
                var markup = new InlineKeyboardBuilder()
                    .CallbackButtonList(
                        schedules,
                        (s, _) => s.Name,
                        (s, _) => $"{button} {s.Id}",
                        rowCount: 2
                     )
                    .CallbackButton("Відмінити", $"delete_message {UpdateContext.MessageId}")
                    .Build();

                await Client.SendTextMessageAsync(
                    text,
                    replyMarkup: markup
                );
            }
        }
    }
}
