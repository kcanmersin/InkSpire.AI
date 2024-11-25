using Core.Data.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Core.Data.Configuration
{
    public class StoryConfiguration : IEntityTypeConfiguration<Story>
    {
        public void Configure(EntityTypeBuilder<Story> builder)
        {
            builder.Property(s => s.Title)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(s => s.Content)
                .IsRequired();

            builder.Property(s => s.IsPublic)
                .IsRequired();

            builder.Property(s => s.PageCount)
                .IsRequired();

            builder.HasOne(s => s.CreatedBy)
                .WithMany()
                .HasForeignKey(s => s.CreatedById)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(s => s.StoryImages)
                .WithOne(si => si.Story)
                .HasForeignKey(si => si.StoryId);

            builder.HasMany(s => s.Comments)
                .WithOne(c => c.Story)
                .HasForeignKey(c => c.StoryId);

            builder.HasMany(s => s.Reactions)
                .WithOne(r => r.Story)
                .HasForeignKey(r => r.StoryId);
        }
    }
}
