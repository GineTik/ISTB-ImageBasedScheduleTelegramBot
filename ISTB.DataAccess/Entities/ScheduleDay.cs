using System.ComponentModel.DataAnnotations;

namespace ISTB.DataAccess.Entities
{
    public class ScheduleDay : Entity
    {
        [Required]
        public uint Position { get; set; }

        public string Description { get; set; }

        [Required]
        public string ImageFileName { get; set; }
    }
}
