using ISTB.BusinessLogic.DTOs.Schedule;
using ISTB.BusinessLogic.Services.Interfaces;
using ISTB.Framework.Attributes.TargetExecutorAttributes;
using ISTB.Framework.Executors;
using ISTB.Framework.Executors.Helpers.Factories.Interfaces;
using ISTB.Framework.MessagePresets.Extensions.AdvancedTelegramBotClient;
using ISTB.Framework.TelegramBotApplication.AdvancedBotClient.Extensions;
using ISTB.TelegramBot.Enum.Buttons;
using ISTB.TelegramBot.MessagePresets.SchedulesMenu;
using Telegram.Bot;

namespace ISTB.TelegramBot.Executors.Schedule
{
    public class RemoveScheduleExecutor : Executor
    {
        private readonly IScheduleService _service;
        private readonly SchedulePresets _presets;
        private readonly IExecutorFactory _factory;

        public RemoveScheduleExecutor(IScheduleService service, SchedulePresets presets, IExecutorFactory factory)
        {
            _service = service;
            _presets = presets;
            _factory = factory;
        }

        [TargetCommands("remove_schedule")]
        public async Task RemoveScheduleCommand()
        {
            var executor = _factory.CreateExecutor<GetScheduleExecutor>();
            await executor.SendSchedules(
                "Виберіть розклад, який бажаєте видалити",
                ScheduleButtons.RemoveSchedule
            );
        }

        [TargetCallbacksDatas(nameof(RemoveSchedule))]
        public async Task RemoveSchedule(int scheduleId, int? messageId)
        {
            await Client.DeleteCallbackQueryMessageAsync();

            await _service.RemoveByIdAsync(new RemoveScheduleByIdDTO()
            {
                Id = scheduleId,
                TelegramUserId = UpdateContext.TelegramUserId
            });

            if (messageId != null)
            {
                var preset = await _presets.GetSchedulesAsync();
                await Client.EditMessageResponseAsync(
                    messageId.Value,
                    preset
                ); 
            }
            else
            {
                await Client.SendTextMessageAsync(
                    "Розклад видалений"
                );
            }
        }
    }
}
