using ISTB.Framework.Attributes.TargetExecutorAttributes;
using ISTB.Framework.Executors;
using ISTB.Framework.MessagePresets.Extensions.AdvancedTelegramBotClient;
using ISTB.Framework.TelegramBotApplication.AdvancedBotClient.Extensions;
using ISTB.TelegramBot.Enum.Buttons;
using ISTB.TelegramBot.MessagePresets.SchedulesMenu;

namespace ISTB.TelegramBot.Executors.ScheduleWeek
{
    public class RemoveWeekExecutor : Executor
    {
        private readonly SchedulePresets _presets;

        public RemoveWeekExecutor(SchedulePresets presets)
        {
            _presets = presets;
        }

        [TargetCallbacksDatas(nameof(WeekButtons.RemoveScheduleWeek))]
        public async Task RemoveWeek(int weekId, int scheduleId, int messageIdToEdit)
        {
            await Client.AnswerCallbackQueryAsync();
            await Client.DeleteCallbackQueryMessageAsync();

            await Client.SendTextMessageAsync($"week id to remove: {weekId}, messageId: {messageIdToEdit}");

            var preset = 
                await _presets.GetScheduleInfoAsync(scheduleId) ??
                await _presets.GetSchedulesAsync();

            await Client.EditMessageResponseAsync(
                messageIdToEdit,
                preset
            );
        }
    }
}
