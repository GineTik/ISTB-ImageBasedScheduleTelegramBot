using ISTB.BusinessLogic.DTOs.ScheduleWeek;
using ISTB.BusinessLogic.Services.Interfaces;
using ISTB.Framework.Attributes.TargetExecutorAttributes;
using ISTB.Framework.Executors;
using ISTB.Framework.TelegramBotApplication.Context;
using ISTB.TelegramBot.Executors.Schedule;

namespace ISTB.TelegramBot.Executors.ScheduleWeek
{
    public class CreateWeekExecutor : Executor
    {
        private readonly IScheduleWeekService _service;

        public CreateWeekExecutor(IScheduleWeekService service)
        {
            _service = service;
        }

        [TargetCallbacksDatas(nameof(CreateWeek))]
        public async Task CreateWeek(int scheduleId, int weekPosition)
        {
            await _service.CreateWeekAsync(new CreateScheduleWeekDTO
            { 
                ScheduleId = scheduleId,
                TelegramUserId = UpdateContext.TelegramUserId,
                Position = weekPosition
            });

            await ExecuteAsync<GetScheduleExecutor>(e => e.ShowSchedule(scheduleId));
        }
    }
}
