using Domain.AggregateObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.EntityFramework.Configuration
{
    public class TaskConfiguration : IEntityTypeConfiguration<TaskModel>
    {
        public void Configure(EntityTypeBuilder<TaskModel> builder)
        {
            builder.HasKey(Task => Task.Id);
            builder.Property(Task => Task.Id).ValueGeneratedOnAdd().UseIdentityColumn();
            builder.Property(Task => Task.Status).IsRequired();
            builder.Property(Task => Task.Responsable);
            builder.Property(Task => Task.EmailResponsable).HasMaxLength(50);
            builder.Property(Task => Task.Objective).IsRequired().HasMaxLength(75);
            builder.Property(Task => Task.Description).HasMaxLength(300);
            builder.Property(Task => Task.CreatedDate).IsRequired();
            builder.Property(Task => Task.EndDate);
            builder.HasOne(Task => Task.User).WithMany().HasForeignKey(T => T.Responsable);
        }
    }
}
