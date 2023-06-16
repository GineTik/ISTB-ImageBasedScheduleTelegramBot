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
        [ParametersSeparator("")]
        [ParseErrorMessages(ArgsLengthIsLess = "Ви забули ввести назву розкладу")]
        public async Task Create(string scheduleName)
        {
            var schedule = await _service.CreateAsync(new CreateScheduleDTO
            {
                Name = scheduleName,
                TelegramUserId = UpdateContext.TelegramUserId
            });
            await Client.SendTextMessageAsync("Створенна нова група з назвою: " + schedule.Name);
        }
    }
}
