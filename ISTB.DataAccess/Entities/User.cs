using System.ComponentModel.DataAnnotations;

namespace ISTB.DataAccess.Entities
{
    public class User : Entity
    {
        [Required]
        public long TelegramUserId { get; set; }

        public int RoleId { get; set; }
        public IEnumerable<Schedule> Groups { get; set; }
    }
}
