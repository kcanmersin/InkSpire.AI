using Core.Data.Entity;
using InkSpire.Core.Data.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Core.Data.Configuration
{
    public class BookConfiguration : IEntityTypeConfiguration<Book>
    {
        public void Configure(EntityTypeBuilder<Book> builder)
        {
            builder.ToTable("Books");
            builder.HasKey(b => b.Id);

            builder.Property(b => b.Title)
                .HasMaxLength(200)
                .IsRequired();

            builder.Property(b => b.AuthorId)
                .IsRequired();
            builder.HasOne<Test>()
               .WithMany()
               .HasForeignKey(b => b.TestId)
               .OnDelete(DeleteBehavior.SetNull);
            builder.HasMany(b => b.Images)
                .WithOne(i => i.Book)
                .HasForeignKey(i => i.BookId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(b => b.Comments)
                .WithOne(c => c.Book)
                .HasForeignKey(c => c.BookId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(b => b.Reactions)
                .WithOne(r => r.Book)
                .HasForeignKey(r => r.BookId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
