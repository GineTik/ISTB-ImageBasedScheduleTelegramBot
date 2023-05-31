using System.ComponentModel.DataAnnotations;

namespace ISTB.DataAccess.Entities
{
    public class Group : Entity
    {
        [Required]
        public string Name { get; set; }

        public User User { get; set; }
        public ICollection<Schedule> Schedules { get; set; }
    }
}
