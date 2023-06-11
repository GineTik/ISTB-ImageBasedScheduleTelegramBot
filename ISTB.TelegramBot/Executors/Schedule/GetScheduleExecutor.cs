using ISTB.BusinessLogic.Services.Interfaces;
using ISTB.Framework.Attributes.TargetExecutorAttributes;
using ISTB.Framework.Executors;
using ISTB.Framework.MessagePresets.Extensions.AdvancedTelegramBotClient;
using ISTB.Framework.TelegramBotApplication.Extensions.AdvancedTelegramBotClient;
using ISTB.TelegramBot.Enum.Buttons;
using ISTB.TelegramBot.MessagePresets.SchedulesMenu;

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
        public async Task GetMySchedule()
        {
            var preset = await _presets.GetSchedulesAsync();
            await Client.SendMessageResponseAsync(preset);
        }

        [TargetCallbacksDatas(nameof(ScheduleButtons.SelectSchedule))]
        public async Task SelectSchedule(int scheduleId)
        {
            await Client.AnswerCallbackQueryAsync();
            var preset = await _presets.GetScheduleInfoAsync(scheduleId);

            if (preset == null)
            {
                // schedule with this id not exists
                await Client.DeleteCurrentCallbackButtonAsync();
            }
            else
            {
                await Client.EditMessageResponseAsync(
                    UpdateContext.Update.CallbackQuery!.Message!.MessageId, 
                    preset
                );
            }
        }

        [TargetCallbacksDatas(nameof(ScheduleButtons.BackToMySchedules))]
        public async Task BackToMySchedules()
        {
            var preset = await _presets.GetSchedulesAsync();
            var messageId = UpdateContext.Update.CallbackQuery!.Message!.MessageId;
            await Client.EditMessageResponseAsync(messageId, preset);
        }
    }
}
