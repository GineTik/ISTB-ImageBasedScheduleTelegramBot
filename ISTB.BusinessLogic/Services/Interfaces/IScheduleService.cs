using ISTB.BusinessLogic.DTOs.Schedule;

namespace ISTB.BusinessLogic.Services.Interfaces
{
    public interface IScheduleService
    {
        Task<ICollection<ScheduleDTO>> GetListByTelegramUserIdAsync(long telegramUserId);
        Task<ScheduleDTO?> GetByNameAsync(GetScheduleByNameDTO dto);
        Task<ScheduleDTO?> GetByIdAsync(int id);
        Task<ScheduleDTO> CreateAsync(CreateScheduleDTO dto);
        Task RemoveByNameAsync(RemoveScheduleDTO dto);
        Task RemoveByIdAsync(RemoveScheduleByIdDTO dto);
        Task ChangeNameAsync(ChangeScheduleNameDTO dto);
    }
}
