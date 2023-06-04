using ISTB.BusinessLogic.DTOs.Group;
using ISTB.BusinessLogic.Services.Interfaces;
using ISTB.Framework.Attributes.TargetExecutorAttributes;
using ISTB.Framework.Attributes.ValidateInputDataAttributes;
using ISTB.Framework.BotApplication.Context;
using ISTB.Framework.Executors;

namespace ISTB.TelegramBot.Executors.Commands.Group
{
    public class RemoveGroupCommandParameters
    {
        public string GroupName { get; set; }
    }

    [TargetCommands("rm_group, rmg")]
    [RequireCorrectParameters(typeof(RemoveGroupCommandParameters), ErrorMessage = "Ви не вказали або вказали некоректно ім'я групи")]
    public class RemoveGroupCommand : CommandExecutor<RemoveGroupCommandParameters>
    {
        private readonly IGroupService _service;

        public RemoveGroupCommand(IGroupService service)
        {
            _service = service;
        }

        public override async Task ExecuteAsync()
        {
            await _service.RemoveGroupAsync(new RemoveGroupDTO
            { 
                Name = Parameters.GroupName,
                TelegramUserId = UpdateContext.TelegramUserId
            });
            await SendTextAsync("Група видалена");
        }
    }
}
