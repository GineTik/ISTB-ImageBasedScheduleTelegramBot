using ISTB.BusinessLogic.DTOs.Group;
using ISTB.BusinessLogic.Services.Interfaces;
using ISTB.Framework.Attributes.TargetExecutorAttributes;
using ISTB.Framework.Executors;

namespace ISTB.TelegramBot.Executors.Commands.Group
{
    [TargetCommand("create_group")]
    public class CreateGroupCommand : Executor
    {
        private readonly IGroupService _service;

        public CreateGroupCommand(IGroupService service)
        {
            _service = service;
        }

        public override async Task ExecuteAsync()
        {
            var group = await _service.CreateGroupAsync(new CreateGroupDTO
            {
                Name = "Group Radom#" + new Random().Next(101, 300),
                TelegramUserId = UpdateContext.TelegramUserId
            });
            await SendTextAsync(group.Name);
        }
    }
}
