using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using  SharpQuiz.Domain.Entity;

namespace SharpQuiz.Database.EntitiesConfig;

public class ChapterConfig : IEntityTypeConfiguration<Chapter>
{
    public void Configure(EntityTypeBuilder<Chapter> builder)
    {
        builder.HasKey(x => x.Id);
        
        builder.HasOne(x => x.Book)
            .WithMany(x => x.Chapters)
            .HasForeignKey(x => x.BookId);
    }
}