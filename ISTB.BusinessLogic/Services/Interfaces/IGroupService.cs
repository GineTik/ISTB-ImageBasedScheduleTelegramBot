using ISTB.BusinessLogic.DTOs.Group;

namespace ISTB.BusinessLogic.Services.Interfaces
{
    public interface IGroupService
    {
        Task<ICollection<GroupDTO>> GetGroupsByTelegramUserIdAsync(long telegramUserId);
        Task<GroupDTO?> GetGroupByNameAsync(GetGroupByNameDTO dto);
        Task<GroupDTO?> GetGroupByIdAsync(int id);
        Task<GroupDTO> CreateGroupAsync(CreateGroupDTO dto);
        Task RemoveGroupAsync(RemoveGroupDTO dto);
        Task RemoveGroupByIdAsync(RemoveGroupByIdDTO dto);
        Task ChangeGroupNameAsync(ChangeGroupNameDTO dto);
    }
}
