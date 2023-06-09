using ISTB.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;

namespace ISTB.DataAccess.EF
{
    public class DataContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Schedule> Schedule { get; set; }
        public DbSet<ScheduleWeek> SchedulesWeeks { get; set; }
        public DbSet<ScheduleDay> SchedulesDays { get; set; }

        public DataContext(DbContextOptions<DataContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Role>().HasData(
                new Role { Id = 1, Name = "Owner" });

            builder.Entity<User>()
                .HasIndex(user => user.TelegramUserId)
                .IsUnique(true);
        }
    }
}
