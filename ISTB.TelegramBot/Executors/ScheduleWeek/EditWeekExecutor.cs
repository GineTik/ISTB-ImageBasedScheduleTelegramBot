using ISTB.BusinessLogic.DTOs.ScheduleWeek;
using ISTB.BusinessLogic.Services.Interfaces;
using ISTB.Framework.Attributes.TargetExecutorAttributes;
using ISTB.Framework.Executors;
using ISTB.Framework.TelegramBotApplication.AdvancedBotClient.Extensions;

namespace ISTB.TelegramBot.Executors.ScheduleWeek
{
    public class EditWeekExecutor : Executor
    {
        private readonly IScheduleWeekService _weekService;

        public EditWeekExecutor(IScheduleWeekService weekService)
        {
            _weekService = weekService;
        }

        [TargetCallbacksDatas(nameof(ChooseCurrentWeek))]
        public async Task ChooseCurrentWeek(int scheduleId, int weekPosition)
        {
            await Client.AnswerCallbackQueryAsync();
            await _weekService.ChooseWeekLikeCurrentAsync(new ChooseCurrentWeekDTO
            {
                ScheduleId = scheduleId,
                WeekPosition = weekPosition
            });
        }
    }
}
