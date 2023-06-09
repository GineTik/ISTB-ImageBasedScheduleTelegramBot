using ISTB.BusinessLogic.DTOs.Group;
using ISTB.BusinessLogic.Services.Interfaces;
using ISTB.Framework.Attributes.TargetExecutorAttributes;
using ISTB.Framework.Executors;

namespace ISTB.TelegramBot.Executors.Group
{
    public class EditGroupExecutor : Executor
    {
        private readonly IGroupService _service;

        public EditGroupExecutor(IGroupService service)
        {
            _service = service;
        }

        [TargetCommands("change_gname, chgn", Description = "Змінити назву групи")]
        public async Task ChangeNameCommand(string oldName, string newName)
        {
            await _service.ChangeGroupNameAsync(new ChangeGroupNameDTO
            {
                OldName = oldName,
                NewName = newName,
                TelegramUserId = UpdateContext.TelegramUserId
            });
            await Client.SendTextResponseAsync("Ім'я групи змінено");
        }
    }
}
