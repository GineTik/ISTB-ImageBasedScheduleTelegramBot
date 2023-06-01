using ISTB.BusinessLogic.DTOs.Group;

namespace ISTB.BusinessLogic.Services.Interfaces
{
    public interface IGroupService
    {
        Task<ICollection<GroupDTO>> GetGroupsByTelegramUserIdAsync(long telegramUserId);
        Task<GroupDTO> CreateGroupAsync(CreateGroupDTO dto);
        Task RemoveGroupAsync(RemoveGroupDTO dto);
    }
}
