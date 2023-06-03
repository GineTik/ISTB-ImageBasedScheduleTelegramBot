using ISTB.BusinessLogic.Services.Interfaces;
using ISTB.Framework.Attributes.TargetExecutorAttributes;
using ISTB.Framework.Executors;

namespace ISTB.TelegramBot.Executors.Commands.Group
{
    public class RemoveGroupCommandParameters
    {
        public string GroupName { get; set; }
    }

    [TargetCommands("rm_group, rmg")]
    public class RemoveGroupCommand : CommandExecutor<RemoveGroupCommandParameters>
    {
        private readonly IGroupService _service;

        public RemoveGroupCommand(IGroupService service)
        {
            _service = service;
        }

        public override Task ExecuteAsync()
        {
            throw new NotImplementedException();
            //_service.RemoveGroupAsync();
        }
    }
}
