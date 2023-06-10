using ISTB.BusinessLogic.DTOs.Schedule;
using ISTB.BusinessLogic.DTOs.ScheduleWeek;
using ISTB.BusinessLogic.Services.Interfaces;
using ISTB.Framework.Attributes.TargetExecutorAttributes;
using ISTB.Framework.Executors;
using ISTB.TelegramBot.Enum.Buttons;
using ISTB.TelegramBot.Helpers;

namespace ISTB.TelegramBot.Executors.ScheduleWeek
{
    public class CreateWeekExecutor : Executor
    {
        private readonly IScheduleService _service;

        public CreateWeekExecutor(IScheduleService service)
        {
            _service = service;
        }

        [TargetCallbacksDatas(nameof(ScheduleButtons.CreateScheduleWeek))]
        public async Task CreateWeek(int scheduleId)
        {
            await _service.CreateWeekAsync(new CreateScheduleWeekDTO
            { 
                ScheduleId = scheduleId,
                TelegramUserId = UpdateContext.TelegramUserId
            });

            await ScheduleHelper.EditScheduleInfoMessageAsync(_service, UpdateContext, scheduleId);
        }
    }
}
