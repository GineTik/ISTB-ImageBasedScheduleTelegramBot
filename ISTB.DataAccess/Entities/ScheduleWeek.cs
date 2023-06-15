namespace ISTB.DataAccess.Entities
{
    public class ScheduleWeek : Entity
    {
        public uint? Position { get; set; }

        public int ScheduleId { get; set; }
        public Schedule Schedule { get; set; }

        public IEnumerable<ScheduleDay> Days { get; set; }
    }
}
