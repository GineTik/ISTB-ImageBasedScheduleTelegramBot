using ISTB.BusinessLogic.DTOs.ScheduleDay;

namespace ISTB.BusinessLogic.Services.Interfaces
{
    public interface IScheduleDayService
    {
        Task<ScheduleDayDTO> GetByIdAsync(int id);
        Task<ScheduleDayDTO> GetTodayByScheduleIdAsync(int id);
        Task<ScheduleDayDTO> GetByDayNumberAsync(GetByDayNumberDTO dto);
        Task EditPhotoAsync(EditPhotoIdDTO dto);
        Task EditDescriptionAsync(EditDescriptionDTO dto);
    }
}
