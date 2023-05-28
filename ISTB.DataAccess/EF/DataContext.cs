using Microsoft.EntityFrameworkCore;

namespace ISTB.DataAccess.EF
{
    public class DataContext : DbContext
    {
        //public DbSet<User> Users { get; set; }
        //....
        //..

        public DataContext(DbContextOptions<DataContext> options) : base(options) { }
    }
}
