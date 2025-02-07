using InkSpire.Core.Data.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Core.Data.Configuration
{
    public class BookImageConfiguration : IEntityTypeConfiguration<BookImage>
    {
        public void Configure(EntityTypeBuilder<BookImage> builder)
        {
            builder.ToTable("BookImages");
            builder.HasKey(x => x.Id);

            builder.Property(x => x.ImageData)
                .IsRequired();

            builder.HasOne(img => img.Book)
                .WithMany(book => book.Images)
                .HasForeignKey(img => img.BookId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
