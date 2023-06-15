using ISTB.BusinessLogic.DTOs.ScheduleDay;

namespace ISTB.BusinessLogic.DTOs.ScheduleWeek
{
    public class ScheduleWeekWithDaysDTO : ScheduleWeekDTO
    {
        public IEnumerable<ScheduleDayDTO> Days { get; set; }
    }
}
