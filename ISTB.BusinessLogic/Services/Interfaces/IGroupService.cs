using ISTB.BusinessLogic.DTOs.Group;

namespace ISTB.BusinessLogic.Services.Interfaces
{
    public interface IGroupService
    {
        ICollection<GroupDTO> GetGroupsByTelegramUserId(long telegramUserId);
    }
}
