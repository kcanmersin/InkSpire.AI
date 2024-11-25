using Core.Data.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Core.Data.Configuration
{
    public class StoryImageConfiguration : IEntityTypeConfiguration<StoryImage>
    {
        public void Configure(EntityTypeBuilder<StoryImage> builder)
        {
            builder.Property(si => si.ImageData)
                .IsRequired();

            builder.Property(si => si.Page)
                .IsRequired();

            builder.HasOne(si => si.Story)
                .WithMany(s => s.StoryImages)
                .HasForeignKey(si => si.StoryId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
