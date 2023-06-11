using ISTB.BusinessLogic.DTOs.Schedule;
using ISTB.BusinessLogic.DTOs.ScheduleWeek;

namespace ISTB.BusinessLogic.Services.Interfaces
{
    public interface IScheduleService
    {
        Task<ICollection<ScheduleDTO>> GetListByTelegramUserIdAsync(long telegramUserId);
        Task<ScheduleDTO?> GetByNameAsync(GetScheduleByNameDTO dto);
        Task<ScheduleDTO?> GetByIdAsync(GetScheduleByIdDTO dto);
        Task<ScheduleWithWeeksDTO?> GetWithWeeksByIdAsync(GetScheduleByIdDTO dto);
        Task<ScheduleWeekDTO?> GetWeekByIdAsync(int weekId);
        Task<ScheduleDTO> CreateAsync(CreateScheduleDTO dto);
        Task<ScheduleWeekDTO> CreateWeekAsync(CreateScheduleWeekDTO dto);
        Task RemoveByNameAsync(RemoveScheduleDTO dto);
        Task RemoveByIdAsync(RemoveScheduleByIdDTO dto);
        Task ChangeNameAsync(ChangeScheduleNameDTO dto);
    }
}
