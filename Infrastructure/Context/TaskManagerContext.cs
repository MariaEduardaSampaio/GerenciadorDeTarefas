using Domain.AggregateObjects;
using Infrastructure.EntityFramework.Configuration;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Context
{
    public class TaskManagerContext : DbContext
    {
        // public TaskManagerContext(DbContextOptions<TaskManagerContext> options): base(options) { }
        public DbSet<TaskModel> Tasks { get; set;}
        public DbSet<UserModel> Users { get; set;}

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) 
        {
            optionsBuilder.UseSqlServer("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=TaskManager;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False");
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new TaskConfiguration());
            modelBuilder.ApplyConfiguration(new UserConfiguration());
        }
    }
}
