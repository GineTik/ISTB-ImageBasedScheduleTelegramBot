using ISTB.BusinessLogic.Services.Interfaces;
using ISTB.Framework.Attributes.TargetExecutorAttributes;
using ISTB.Framework.Executors;
using ISTB.Framework.Executors.Helpers.Factories.Executors;
using ISTB.TelegramBot.Executors.Schedule;
using ISTB.TelegramBot.Views.ScheduleWeek;

namespace ISTB.TelegramBot.Executors.ScheduleWeek
{
    public class GetWeekExecutor : Executor
    {
        private readonly IScheduleWeekService _weekService;
        private readonly IExecutorFactory _factory;

        public GetWeekExecutor(IScheduleWeekService weekService, IExecutorFactory factory)
        {
            _weekService = weekService;
            _factory = factory;
        }

        [TargetCallbacksDatas(nameof(ShowWeek))]
        public async Task ShowWeek(int weekId)
        {
            var week = await _weekService.GetWeekByIdAsync(weekId);

            if (week == null)
            {
                await ExecuteAsync<GetScheduleExecutor>(e => e.ShowMySchedules());
            }
            else
            {
                await ExecuteAsync<ShowWeekView>(v => v.ShowWeek(week));
            }
        }
    }
}
