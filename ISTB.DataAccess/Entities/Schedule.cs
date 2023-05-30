namespace ISTB.DataAccess.Entities
{
    public class Schedule : Entity
    {
        public uint Position { get; set; }
        public ICollection<ScheduleDay> Days { get; set; }
    }
}
