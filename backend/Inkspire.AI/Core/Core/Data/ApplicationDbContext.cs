using Core.Data.Configuration;
using Core.Data.Entity;
using Core.Data.Entity.User;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Core.Data
{
    public class ApplicationDbContext : IdentityDbContext<AppUser, AppRole, Guid>
    {
        public DbSet<Story> Stories { get; set; }
        public DbSet<StoryImage> StoryImages { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Reaction> Reactions { get; set; }


        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new AppUserConfiguration());
            modelBuilder.ApplyConfiguration(new AppRoleConfiguration());
            modelBuilder.ApplyConfiguration(new StoryConfiguration());
            modelBuilder.ApplyConfiguration(new StoryImageConfiguration());
            modelBuilder.ApplyConfiguration(new CommentConfiguration());
            modelBuilder.ApplyConfiguration(new ReactionConfiguration());

            SeedData(modelBuilder);

            base.OnModelCreating(modelBuilder);
        }

        private void SeedData(ModelBuilder modelBuilder)
        {
            var superAdminRoleId = Guid.NewGuid();
            var adminRoleId = Guid.NewGuid();
            var userRoleId = Guid.NewGuid();

            modelBuilder.Entity<AppRole>().HasData(
                new AppRole { Id = superAdminRoleId, Name = "SUPERADMIN", NormalizedName = "SUPERADMIN" },
                new AppRole { Id = adminRoleId, Name = "ADMIN", NormalizedName = "ADMIN" },
                new AppRole { Id = userRoleId, Name = "USER", NormalizedName = "USER" }
            );

            var passwordHasher = new PasswordHasher<AppUser>();

            var superAdmin = new AppUser
            {
                Id = Guid.NewGuid(),
                UserName = "SUPERADMIN",
                NormalizedUserName = "SUPERADMIN",
                Email = "superadmin@example.com",
                NormalizedEmail = "SUPERADMIN@EXAMPLE.COM",
                Name = "SUPERADMIN",
                Surname = "USER",
                IsConfirmed = true
            };
            superAdmin.PasswordHash = passwordHasher.HashPassword(superAdmin, "19071907");

            var admin = new AppUser
            {
                Id = Guid.NewGuid(),
                UserName = "ADMIN",
                NormalizedUserName = "ADMIN",
                Email = "admin@example.com",
                NormalizedEmail = "ADMIN@EXAMPLE.COM",
                Name = "ADMIN",
                Surname = "USER",
                IsConfirmed = true
            };
            admin.PasswordHash = passwordHasher.HashPassword(admin, "19071907");

            var user = new AppUser
            {
                Id = Guid.NewGuid(),
                UserName = "USER",
                NormalizedUserName = "USER",
                Email = "user@example.com",
                NormalizedEmail = "USER@EXAMPLE.COM",
                Name = "USER",
                Surname = "USER",
                IsConfirmed = true
            };
            user.PasswordHash = passwordHasher.HashPassword(user, "19071907");

            modelBuilder.Entity<AppUser>().HasData(superAdmin, admin, user);

            modelBuilder.Entity<IdentityUserRole<Guid>>().HasData(
                new IdentityUserRole<Guid> { UserId = superAdmin.Id, RoleId = superAdminRoleId },
                new IdentityUserRole<Guid> { UserId = admin.Id, RoleId = adminRoleId },
                new IdentityUserRole<Guid> { UserId = user.Id, RoleId = userRoleId }
            );
        }
    }
}
