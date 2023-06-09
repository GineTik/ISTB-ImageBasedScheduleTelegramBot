using ISTB.BusinessLogic.DTOs.Group;
using ISTB.BusinessLogic.Services.Interfaces;
using ISTB.Framework.Attributes.TargetExecutorAttributes;
using ISTB.Framework.BotApplication.Context;
using ISTB.Framework.Executors;

namespace ISTB.TelegramBot.Executors.Group
{
    public class CreateGroupExecutor : Executor
    {
        private readonly IGroupService _service;

        public CreateGroupExecutor(IGroupService service)
        {
            _service = service;
        }

        [TargetCommands("create_group, cg")]
        public async Task CreateGroupCommand(string groupName)
        {
            var group = await _service.CreateGroupAsync(new CreateGroupDTO
            {
                Name = groupName,
                TelegramUserId = UpdateContext.TelegramUserId
            });
            await Client.SendTextResponseAsync("Створенна нова група з назвою: " + group.Name);
        }
    }
}
