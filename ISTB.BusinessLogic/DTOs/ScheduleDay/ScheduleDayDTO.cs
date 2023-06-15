using Microsoft.VisualBasic;

namespace ISTB.BusinessLogic.DTOs.ScheduleDay
{
    public class ScheduleDayDTO
    {
        public int? Id { get; set; }
        public string ImageFileUrl{ get; set; }
        public string Name => DateAndTime.WeekdayName(Position + 1);
        public int Position { get; set; }
        public string Description { get; set; } = default!;
    }
}
