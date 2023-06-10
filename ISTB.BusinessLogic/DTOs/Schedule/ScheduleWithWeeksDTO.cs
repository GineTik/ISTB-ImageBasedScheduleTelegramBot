using ISTB.BusinessLogic.DTOs.ScheduleWeek;

namespace ISTB.BusinessLogic.DTOs.Schedule
{
    public class ScheduleWithWeeksDTO : ScheduleDTO
    {
        public IEnumerable<ScheduleWeekDTO> Weeks { get; set; }
    }
}
