using ISTB.BusinessLogic.DTOs.Group;
using ISTB.BusinessLogic.Services.Interfaces;
using ISTB.Framework.Attributes.TargetExecutorAttributes;
using ISTB.Framework.Executors;

namespace ISTB.TelegramBot.Executors.Commands
{
    public class GroupCommands : Executor
    {
        private readonly IGroupService _service;

        public GroupCommands(IGroupService service)
        {
            _service = service;
        }

        [TargetCommands("create_group, cg")]
        public async Task CreateGroup(string groupName)
        {
            var group = await _service.CreateGroupAsync(new CreateGroupDTO
            {
                Name = groupName,
                TelegramUserId = UpdateContext.TelegramUserId
            });
            await SendTextAsync("Створенна нова група з назвою: " + group.Name);
        }
        
        [TargetCommands("remove_group, rmg")]
        public async Task RemoveGroup(string groupName)
        {
            await _service.RemoveGroupAsync(new RemoveGroupDTO
            {
                Name = groupName,
                TelegramUserId = UpdateContext.TelegramUserId
            });
            await SendTextAsync("Група видалена");
        }

        [TargetCommands("get_my_groups, gmg")]
        public async Task GetMyGroups()
        {
            var groups = await _service.GetGroupsByTelegramUserIdAsync(UpdateContext.TelegramUserId);

            if (groups.Count() == 0)
                await SendTextAsync("Ви ще не створили групу");
            else
                await SendTextAsync(String.Join("\n", groups.Select(g => g.Name)));
        }

        [TargetCommands("change_gname, chgn", Description = "Змінити назву групи")]
        public async Task ChangeName(string oldName, string newName)
        {
            await _service.ChangeGroupNameAsync(new ChangeGroupNameDTO
            {
                OldName = oldName,
                NewName = newName,
                TelegramUserId = UpdateContext.TelegramUserId
            });
            await SendTextAsync("Ім'я групи змінено");
        }
    }
}
