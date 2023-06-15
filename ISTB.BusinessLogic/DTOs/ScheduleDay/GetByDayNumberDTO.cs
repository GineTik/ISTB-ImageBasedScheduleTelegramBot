using System.ComponentModel.DataAnnotations;

namespace ISTB.BusinessLogic.DTOs.ScheduleDay
{
    public class GetByDayNumberDTO
    {
        public int DayNumber { get; set; }
        public int WeekId { get; set; }
    }
}
