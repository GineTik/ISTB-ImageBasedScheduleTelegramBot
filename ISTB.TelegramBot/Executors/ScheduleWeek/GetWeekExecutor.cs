using ISTB.Framework.Attributes.TargetExecutorAttributes;
using ISTB.Framework.Executors;
using ISTB.Framework.MessagePresets.Extensions.AdvancedTelegramBotClient;
using ISTB.TelegramBot.Enum.Buttons;
using ISTB.TelegramBot.MessagePresets.SchedulesMenu;

namespace ISTB.TelegramBot.Executors.ScheduleWeek
{
    public class GetWeekExecutor : Executor
    {
        private readonly SchedulePresets _presets;

        public GetWeekExecutor(SchedulePresets presets)
        {
            _presets = presets;
        }

        [TargetCallbacksDatas(nameof(ScheduleButtons.SelectScheduleWeek))]
        public async Task GetScheduleWeek(int weekId)
        {
            var messageId = UpdateContext.Update.CallbackQuery!.Message!.MessageId;

            var preset =
                await _presets.GetScheduleWeekInfoAsync(weekId, messageId) ??
                await _presets.GetSchedulesAsync();

            await Client.EditMessageResponseAsync(
                messageId,
                preset
            );
        }
    }
}
