using ISTB.BusinessLogic.DTOs.ScheduleWeek;
using ISTB.BusinessLogic.Services.Interfaces;
using ISTB.Framework.Attributes.TargetExecutorAttributes;
using ISTB.Framework.Executors;
using ISTB.Framework.MessagePresets.Extensions.AdvancedTelegramBotClient;
using ISTB.TelegramBot.Enum.Buttons;
using ISTB.TelegramBot.MessagePresets.SchedulesMenu;

namespace ISTB.TelegramBot.Executors.ScheduleWeek
{
    public class CreateWeekExecutor : Executor
    {
        private readonly IScheduleService _service;
        private readonly SchedulePresets _presets;

        public CreateWeekExecutor(IScheduleService service, SchedulePresets presets)
        {
            _service = service;
            _presets = presets;
        }

        [TargetCallbacksDatas(nameof(ScheduleButtons.CreateScheduleWeek))]
        public async Task CreateWeek(int scheduleId)
        {
            await _service.CreateWeekAsync(new CreateScheduleWeekDTO
            { 
                ScheduleId = scheduleId,
                TelegramUserId = UpdateContext.TelegramUserId
            });

            var preset = await _presets.GetScheduleInfoAsync(scheduleId);
            await Client.EditMessageResponseAsync(
                UpdateContext.Update.CallbackQuery!.Message!.MessageId,
                preset!
            );
        }
    }
}
