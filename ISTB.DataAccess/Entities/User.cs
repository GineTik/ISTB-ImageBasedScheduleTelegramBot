using System.ComponentModel.DataAnnotations;

namespace ISTB.DataAccess.Entities
{
    public class User : Entity
    {
        [Required]
        public long TelegramUserId { get; set; }

        public ICollection<Role> Roles { get; set; }
        public ICollection<Group> Groups { get; set; }
    }
}
