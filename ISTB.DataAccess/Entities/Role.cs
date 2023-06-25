using System.ComponentModel.DataAnnotations;

namespace ISTB.DataAccess.Entities
{
    public class Role : Entity
    {
        [Required]
        public string Name { get; set; }

        public IEnumerable<User> Users { get; set; }
    }
}
