using ISTB.BusinessLogic.DTOs.Schedule;
using ISTB.BusinessLogic.Services.Interfaces;
using ISTB.Framework.Attributes.ParametersParse;
using ISTB.Framework.Attributes.TargetExecutorAttributes;
using ISTB.Framework.TelegramBotApplication.Context;
using ISTB.Framework.Executors;
using ISTB.Framework.TelegramBotApplication.Extensions.AdvancedTelegramBotClient;

namespace ISTB.TelegramBot.Executors.Schedule
{
    public class CreateScheduleExecutor : Executor
    {
        private readonly IScheduleService _service;

        public CreateScheduleExecutor(IScheduleService service)
        {
            _service = service;
        }

        [TargetCommands("create_schedule, cs")]
        [ParametersSeparator(", ")]
        public async Task Create(string groupName)
        {
            var schedule = await _service.CreateAsync(new CreateScheduleDTO
            {
                Name = groupName,
                TelegramUserId = UpdateContext.TelegramUserId
            });
            await Client.SendTextResponseAsync("Створенна нова група з назвою: " + schedule.Name);
        }
    }
}
