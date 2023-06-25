using ISTB.BusinessLogic.DTOs.Schedule;
using ISTB.BusinessLogic.Services.Interfaces;
using ISTB.Framework.Attributes.TargetExecutorAttributes;
using ISTB.Framework.Executors;
using ISTB.Framework.TelegramBotApplication.Context;
using ISTB.TelegramBot.Views.Schedule;

namespace ISTB.TelegramBot.Executors.Schedule
{
    public class GetScheduleExecutor : Executor
    {
        private readonly IScheduleService _service;

        public GetScheduleExecutor(IScheduleService service)
        {
            _service = service;
        }

        [TargetCommands("schedules")]
        [TargetCallbacksDatas(nameof(ShowMySchedules))]
        public async Task ShowMySchedules()
        {
            var schedules = await _service.GetListByTelegramUserIdAsync(UpdateContext.TelegramUserId);
            await ExecuteAsync<ShowScheduleView>(v => v.ShowMySchedules(schedules));
        }

        [TargetCallbacksDatas(nameof(ShowSchedule))]
        public async Task ShowSchedule(int scheduleId)
        {
            var schedule = await _service.GetWithWeeksByIdAsync(new GetScheduleByIdDTO()
            {
                Id = scheduleId,
                TelegramUserId = UpdateContext.TelegramUserId,
            });

            if (schedule == null)
            {
                await ShowMySchedules();
            }
            else
            {
                await ExecuteAsync<ShowScheduleView>(v => v.ShowSchedule(schedule));
            }
        }
    }
}
