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

    [TargetCommands("create_group")]
    [ValidateParameters(typeof(CreateGroupCommandParameters))]
    public class CreateGroupCommand : CommandExecutor<CreateGroupCommandParameters>
    {
        private readonly IGroupService _service;

        public CreateGroupCommand(IGroupService service)
        {
            _service = service;
        }

        public override async Task ExecuteAsync()
        {
            if (Parameters?.GroupName is not { } name)
            {
                await SendTextAsync("Ви не вказали ім'я групи!");
                return;
            }

            var group = await _service.CreateGroupAsync(new CreateGroupDTO
            {
                Name = name,
                TelegramUserId = UpdateContext.TelegramUserId
            });
            await SendTextAsync("Створенна нова група з назвою: " + group.Name);
        }
    }
}
