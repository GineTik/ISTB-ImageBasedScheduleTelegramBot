using ISTB.BusinessLogic.DTOs.Schedule;
using ISTB.BusinessLogic.Services.Interfaces;
using ISTB.Framework.Attributes.TargetExecutorAttributes;
using ISTB.Framework.Executors;
using ISTB.Framework.Executors.Helpers.Factories.Executors;
using ISTB.Framework.TelegramBotApplication.AdvancedBotClient.Extensions;
using ISTB.TelegramBot.Views.Schedule;
using Telegram.Bot;

namespace ISTB.TelegramBot.Executors.Schedule
{
    public class RemoveScheduleExecutor : Executor
    {
        private readonly IScheduleService _service;
        private readonly IExecutorFactory _factory;

        public RemoveScheduleExecutor(IScheduleService service, IExecutorFactory factory)
        {
            _service = service;
            _factory = factory;
        }

        [TargetCommands("remove_schedule")]
        public async Task ShowSchedulesToRemove()
        {
            var schedules = await _service.GetListByTelegramUserIdAsync(UpdateContext.TelegramUserId);
            await ExecuteAsync<RemoveScheduleView>(v => v.ChooseScheduleToRemove(schedules));
        }

        [TargetCallbacksDatas(nameof(RemoveSchedule))]
        public async Task RemoveSchedule(int scheduleId)
        {
            await Client.DeleteMessageAsync();

            await _service.RemoveByIdAsync(new RemoveScheduleByIdDTO()
            {
                Id = scheduleId,
                TelegramUserId = UpdateContext.TelegramUserId
            });

            await ExecuteAsync<RemoveScheduleView>(v => v.ScheduleRemoved());
        }
    }
}
