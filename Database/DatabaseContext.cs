using Microsoft.EntityFrameworkCore;
using SharpQuiz.Database.EntitiesConfig;
using SharpQuiz.Domain.Entity;

namespace SharpQuiz.Database;

public class DatabaseContext : DbContext
{
    public const string SchemaName = "public";
    
    public virtual DbSet<Book> Books { get; set; } = default!;
    
    public virtual DbSet<Chapter> Chapters { get; set; } = default!;
    
    public virtual DbSet<Clause> Clauses { get; set; } = default!;
    
    public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
    { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema(SchemaName); 
        
        modelBuilder.ApplyConfiguration(new BookConfig());
        modelBuilder.ApplyConfiguration(new ChapterConfig());
        modelBuilder.ApplyConfiguration(new ClauseConfig());
        
        base.OnModelCreating(modelBuilder);
    }
}