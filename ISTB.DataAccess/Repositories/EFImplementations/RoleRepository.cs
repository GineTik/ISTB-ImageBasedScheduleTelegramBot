using ISTB.DataAccess.EF;
using ISTB.DataAccess.Entities;
using ISTB.DataAccess.Repositories.Interfaces;

namespace ISTB.DataAccess.Repositories.EFImplementations
{
    public class RoleRepository : EFRepository<Role>, IRoleRepository
    {
        public RoleRepository(DataContext context) : base(context)
        {
        }
    }
}
