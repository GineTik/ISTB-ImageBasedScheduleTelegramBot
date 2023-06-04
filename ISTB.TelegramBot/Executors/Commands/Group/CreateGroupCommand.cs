using ISTB.BusinessLogic.DTOs.Group;
using ISTB.BusinessLogic.Services.Interfaces;
using ISTB.Framework.Attributes.TargetExecutorAttributes;
using ISTB.Framework.Attributes.ValidateInputDataAttributes;
using ISTB.Framework.Executors;

namespace ISTB.TelegramBot.Executors.Commands.Group
{
    public class CreateGroupCommandParameters
    {
        public string GroupName { get; set; }
    }

    [TargetCommands("create_group, cg")]
    [RequireCorrectParameters(typeof(CreateGroupCommandParameters), ErrorMessage = "Ви забули або некоректно ввели назву групи")]
    public class CreateGroupCommand : CommandExecutor<CreateGroupCommandParameters>
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
                Name = Parameters.GroupName,
                TelegramUserId = UpdateContext.TelegramUserId
            });
            await SendTextAsync("Створенна нова група з назвою: " + group.Name);
        }
    }
}
