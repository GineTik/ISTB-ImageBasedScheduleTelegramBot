using ISTB.BusinessLogic.DTOs.ScheduleDay;

namespace ISTB.BusinessLogic.Services.Interfaces
{
    public interface IScheduleDayService
    {
        Task<ScheduleDayDTO> GetByDayNumber(GetByDayNumberDTO dto);
    }
}
