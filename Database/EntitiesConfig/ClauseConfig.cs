using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using  SharpQuiz.Domain.Entity;

namespace SharpQuiz.Database.EntitiesConfig;

public class ClauseConfig : IEntityTypeConfiguration<Clause>
{
    public void Configure(EntityTypeBuilder<Clause> builder)
    {
        builder.HasKey(x => x.Id);
        
        builder.HasOne(x => x.Chapter)
            .WithMany(x => x.Clauses)
            .HasForeignKey(x => x.ChapterId);
    }
}