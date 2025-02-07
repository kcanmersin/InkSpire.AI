using Core.Data.Configuration;
using Core.Data.Entity.User;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using Core.Data.Entity;
using InkSpire.Core.Data.Entity;

namespace Core.Data
{
    public class ApplicationDbContext : IdentityDbContext<AppUser, AppRole, Guid>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<Book> Books { get; set; }
        public DbSet<BookImage> BookImages { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Reaction> Reactions { get; set; }

        //test questions
        public DbSet<Question> Questions { get; set; }

        //test
        public DbSet<Test> Test { get; set; }

        //wrods
        public DbSet<Word> Words { get; set; }



        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new BookConfiguration());
            modelBuilder.ApplyConfiguration(new BookImageConfiguration());
            modelBuilder.ApplyConfiguration(new CommentConfiguration());
            modelBuilder.ApplyConfiguration(new ReactionConfiguration());

            modelBuilder.ApplyConfiguration(new TestConfiguration());
            modelBuilder.ApplyConfiguration(new QuestionConfiguration());

            SeedData(modelBuilder);

            base.OnModelCreating(modelBuilder);
        }

        private void SeedData(ModelBuilder modelBuilder)
        {
            var superAdminRoleId = Guid.Parse("C7A7A7A7-A7A7-A7A7-A7A7-A7A7A7A7A7A7");
            var adminRoleId = Guid.Parse("B7B7B7B7-B7B7-B7B7-B7B7-B7B7B7B7B7B7");
            var userRoleId = Guid.Parse("A7A7A7A7-A7A7-A7A7-A7A7-A7A7A7A7A7A7");

            modelBuilder.Entity<AppRole>().HasData(
                new AppRole { Id = superAdminRoleId, Name = "SUPERADMIN", NormalizedName = "SUPERADMIN" },
                new AppRole { Id = adminRoleId, Name = "ADMIN", NormalizedName = "ADMIN" },
                new AppRole { Id = userRoleId, Name = "USER", NormalizedName = "USER" }
            );

            var superAdminId = Guid.Parse("D7D7D7D7-D7D7-D7D7-D7D7-D7D7D7D7D7D7");
            var adminId = Guid.Parse("E7E7E7E7-E7E7-E7E7-E7E7-E7E7E7E7E7E7");
            var userId = Guid.Parse("F7F7F7F7-F7F7-F7F7-F7F7-F7F7F7F7F7F7");

            var passwordHasher = new PasswordHasher<AppUser>();

            var superAdmin = new AppUser
            {
                Id = superAdminId,
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
                Id = adminId,
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
                Id = userId,
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
                new IdentityUserRole<Guid> { UserId = superAdminId, RoleId = superAdminRoleId },
                new IdentityUserRole<Guid> { UserId = adminId, RoleId = adminRoleId },
                new IdentityUserRole<Guid> { UserId = userId, RoleId = userRoleId }
            );
        }
    }
}
