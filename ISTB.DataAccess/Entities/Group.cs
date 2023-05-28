namespace ISTB.DataAccess.Entities
{
    public class Group : Entity
    {
        public string Name { get; set; }
        public ICollection<Schedule> Schedules { get; set; }
    }
}
