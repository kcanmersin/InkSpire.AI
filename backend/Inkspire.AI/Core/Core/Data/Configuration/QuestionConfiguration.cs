using Core.Data.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Text.Json;

public class QuestionConfiguration : IEntityTypeConfiguration<Question>
{
    public void Configure(EntityTypeBuilder<Question> builder)
    {
        builder.ToTable("Questions");

        builder.HasKey(q => q.Id);

        builder.Property(q => q.QuestionText)
               .IsRequired()
               .HasMaxLength(500); 
        builder.Property(q => q.QuestionType)
               .IsRequired();

        builder.Property(q => q.Score)
               .IsRequired();

        builder.Property(q => q.Answer)
               .HasMaxLength(1000);
        //feedback
        //builder.Property(q => q.Feedback)
        //       .HasMaxLength(1000);
        //mkae feedback nullable
        builder.Property(q => q.Feedback)
               .HasMaxLength(1000)
               .IsRequired(false);
        builder.Property(q => q.Choices)
               .HasConversion(
                   choices => JsonSerializer.Serialize(choices, (JsonSerializerOptions)null),
                   choices => JsonSerializer.Deserialize<List<string>>(choices, (JsonSerializerOptions)null)
               )
               .Metadata.SetValueComparer(new ValueComparer<List<string>>(
                   (c1, c2) => c1.SequenceEqual(c2), 
                   c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())), 
                   c => c.ToList() 
               ));

        builder.Property(q => q.Choices).HasColumnType("json");



    }
}
