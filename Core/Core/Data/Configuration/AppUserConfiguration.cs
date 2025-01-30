using Core.Data.Entity.User;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Core.Data.Configuration
{
    public class AppUserConfiguration : IEntityTypeConfiguration<AppUser>
    {
        public void Configure(EntityTypeBuilder<AppUser> builder)
        {
            builder.Property(u => u.Name)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(u => u.Surname)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(u => u.IsConfirmed)
                .IsRequired();

            builder.Property(u => u.IsTwoFactorEnabled)
                .IsRequired();

            builder.Property(u => u.TwoFactorCode)
                .IsRequired()
                .HasMaxLength(6);

            builder.Property(u => u.TwoFactorExpiryTime)
                .IsRequired(false);

            builder.HasIndex(u => u.Email)
                .IsUnique();
                

        }
    }
}
