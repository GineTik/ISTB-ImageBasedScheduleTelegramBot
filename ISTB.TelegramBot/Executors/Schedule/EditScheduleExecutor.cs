using ISTB.BusinessLogic.DTOs.Schedule;
using ISTB.BusinessLogic.Services.Interfaces;
using ISTB.Framework.Attributes.TargetExecutorAttributes;
using ISTB.Framework.Executors;
using ISTB.Framework.Executors.Factories.Interfaces;
using ISTB.Framework.TelegramBotApplication.Extensions.AdvancedTelegramBotClient;
using ISTB.TelegramBot.Enum.Buttons;

namespace ISTB.TelegramBot.Executors.Schedule
{
    public class EditScheduleExecutor : Executor
    {
        private readonly IScheduleService _service;
        private readonly IExecutorFactory _factory;

        public EditScheduleExecutor(IScheduleService service, IExecutorFactory factory)
        {
            _service = service;
            _factory = factory;
        }

        [TargetCommands("change_schedule_name", Description = "Змінити назву розкладу")]
        public async Task ChangeName()
        {
            var executor = _factory.CreateExecutor<GetScheduleExecutor>();
            await executor.SendSchedules(
                "Виберіть розклад, ім'я якого ви хочете змінити", 
                ScheduleButtons.SelectScheduleToChangeName
            );
        }

        [TargetCallbacksDatas(nameof(ScheduleButtons.SelectScheduleToChangeName))]
        public async Task SelectScheduleToChangeName(int scheduleId)
        {
            await Client.DeleteCallbackQueryMessageAsync();

            var schedule = await _service.GetByIdAsync(new GetScheduleByIdDTO
            {
                Id = scheduleId,
                TelegramUserId = UpdateContext.TelegramUserId
            });

            await Client.SendTextMessageAsync(
                $"Топчик але системи діалогів ще нема (розклад {schedule!.Name})"
            );
        }
    }
}
