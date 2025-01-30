﻿using Core.Data.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

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
        //generalfeedback
        builder.Property(t => t.GeneralFeedback)
               .HasMaxLength(1000);

        builder.Property(t => t.BookId)
               .IsRequired();

        builder.Property(t => t.UserId)
               .IsRequired();


    }
}
