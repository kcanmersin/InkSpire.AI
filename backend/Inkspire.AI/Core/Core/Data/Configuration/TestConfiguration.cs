using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Core.Data.Entity;

public class TestConfiguration : IEntityTypeConfiguration<Test>
{
    public void Configure(EntityTypeBuilder<Test> builder)
    {
        builder.ToTable("Tests");

        builder.HasKey(t => t.Id);

        builder.HasMany(t => t.Questions)
               .WithOne()
               .HasForeignKey("TestId")
               .OnDelete(DeleteBehavior.Cascade);

        builder.Property(t => t.TotalScore)
               .IsRequired();

        builder.Property(t => t.GeneralFeedback)
               .HasMaxLength(1000)
               .IsRequired(false); 
        builder.Property(t => t.BookId)
               .IsRequired();
    }
}
