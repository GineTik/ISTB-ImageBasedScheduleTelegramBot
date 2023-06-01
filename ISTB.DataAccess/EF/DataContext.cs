using ISTB.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;
using System;

namespace ISTB.DataAccess.EF
{
    public class DataContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Group> Groups { get; set; }
        public DbSet<Schedule> Schedules { get; set; }
        public DbSet<ScheduleDay> ScheduleDays { get; set; }

        public DataContext(DbContextOptions<DataContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Role>().HasData(
                new Role { Id = 1, Name = "Own" });

            builder.Entity<User>()
                .HasIndex(user => user.TelegramUserId)
                .IsUnique(true);
        }
    }
}
