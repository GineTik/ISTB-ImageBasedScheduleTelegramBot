using ISTB.BusinessLogic.Services.Interfaces;
using ISTB.Framework.Attributes.TargetExecutorAttributes;
using ISTB.Framework.Executors;

namespace ISTB.TelegramBot.Executors.Commands.Group
{
    [TargetCommand("get_my_groups")]
    public class GetMyGroupsCommand : Executor
    {
        private readonly IGroupService _service;

        public GetMyGroupsCommand(IGroupService service)
        {
            _service = service;
        }

        public override async Task ExecuteAsync()
        {
            var groups = _service.GetGroupsByTelegramUserId(UpdateContext.TelegramUserId);

            if (groups.Count() == 0)
                await SendTextAsync("Ви ще не створили групу");
            else
                await SendTextAsync(String.Join("\n", groups.Select(g => g.Name)));
        }
    }
}
