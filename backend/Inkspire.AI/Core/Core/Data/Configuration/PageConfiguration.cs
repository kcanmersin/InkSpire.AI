using Core.Data.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Core.Data.Configuration
{
    public class PageConfiguration : IEntityTypeConfiguration<Page>
    {
        public void Configure(EntityTypeBuilder<Page> builder)
        {
            builder.Property(p => p.PageNumber)
                .IsRequired();

            builder.Property(p => p.Content)
                .IsRequired()
                .HasMaxLength(50000); 

            builder.HasOne(p => p.Story)
                .WithMany(s => s.Pages)
                .HasForeignKey(p => p.StoryId)
                .OnDelete(DeleteBehavior.Cascade); 
        }
    }
}
