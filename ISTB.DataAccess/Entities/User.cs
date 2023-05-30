namespace ISTB.DataAccess.Entities
{
    public class User : Entity
    {
        public string UserName { get; set; }
        public ICollection<Role> Roles { get; set; }
        public ICollection<Group> Groups { get; set; }
    }
}
