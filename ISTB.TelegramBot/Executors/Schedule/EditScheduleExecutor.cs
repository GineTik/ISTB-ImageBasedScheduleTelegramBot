using ISTB.BusinessLogic.DTOs.Schedule;
using ISTB.BusinessLogic.Services.Interfaces;
using ISTB.Framework.Attributes.TargetExecutorAttributes;
using ISTB.Framework.Executors;

namespace ISTB.TelegramBot.Executors.Schedule
{
    public class EditScheduleExecutor : Executor
    {
        private readonly IScheduleService _service;

        public EditScheduleExecutor(IScheduleService service)
        {
            _service = service;
        }

        [TargetCommands("change_sname, chsn", Description = "Змінити назву розкладу")]
        public async Task ChangeNameCommand(string oldName, string newName)
        {
            await _service.ChangeNameAsync(new ChangeScheduleNameDTO
            {
                OldName = oldName,
                NewName = newName,
                TelegramUserId = UpdateContext.TelegramUserId
            });
            await Client.SendTextResponseAsync("Ім'я розкладу змінено");
        }
    }
}
