using Domain.AggregateObjects;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.EntityFramework.Configuration
{
    public class UserConfiguration : IEntityTypeConfiguration<UserModel>
    {
        public void Configure(EntityTypeBuilder<UserModel> builder)
        {
            builder.HasKey(User => User.Id);
            builder.Property(User => User.Id).ValueGeneratedOnAdd().UseIdentityColumn();
            builder.Property(User => User.Name).IsRequired().HasMaxLength(120);
            builder.Property(User => User.Email).IsRequired().HasMaxLength(50);
            builder.Property(User => User.Password).IsRequired().HasMaxLength(50);
            builder.Property(User => User.Role).IsRequired();
            builder.Property(User => User.AccessType);
            builder.HasMany(user => user.Tasks).WithOne(task => task.User).HasForeignKey(task => task.UserID);
        }
    }
}
