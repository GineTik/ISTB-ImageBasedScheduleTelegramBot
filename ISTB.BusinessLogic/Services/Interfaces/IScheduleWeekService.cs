using ISTB.BusinessLogic.DTOs.ScheduleWeek;

namespace ISTB.BusinessLogic.Services.Interfaces
{
    public interface IScheduleWeekService
    {
        Task<ScheduleWeekDTO> CreateWeekAsync(CreateScheduleWeekDTO dto);
        Task<ScheduleWeekWithDaysDTO?> GetWeekByIdAsync(int weekId);
        Task<IEnumerable<ScheduleWeekDTO>> GetWeeks(int scheduleId);
    }
}
