using System.ComponentModel.DataAnnotations;

namespace ISTB.DataAccess.Entities
{
    public class ScheduleDay : Entity
    {
        [Required]
        public int Position { get; set; }

        public string? Description { get; set; }

        public string? ImageFileUrl { get; set; }

        public int ScheduleWeekId { get; set; }
    }
}
