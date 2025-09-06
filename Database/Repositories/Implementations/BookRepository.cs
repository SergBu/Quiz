using Microsoft.EntityFrameworkCore;
using SharpQuiz.Database.Repositories.Interfaces;
using SharpQuiz.Domain.Entity;

namespace SharpQuiz.Database.Repositories.Implementations;

public class BookRepository : IBookRepository
{
    private readonly DatabaseContext _context;

    public BookRepository(DatabaseContext context)
    {
        _context = context;
    }

    public async Task<Book?> GetAsync(string bookId) =>
        await _context.Books.FirstOrDefaultAsync(x => x.Id.ToString() == bookId);

    public async Task<Book> CreateAsync(string reportData, IEnumerable<string> templateCodes)
    {
        var newBook = new Book
        {
    // public int Id { get; set; }
    //
    // public int Number { get; set; }
    //
    // public string Name { get; set; }  = null!;
    //
    // public string Type { get; set; }  = null!;
    // public List<Chapter> Chapters { get; set; } = null!;
            // Id = Guid.NewGuid().ToString(),
            // ReportId = 1,
            // TemplateCodes = templateCodes.First(), //Пока что только 1-Й
            // ReportData = reportData,
            // Status = JobStatus.ToDo,
            // CreatedAt = DateTimeOffset.UtcNow,
            // UpdatedAt = DateTimeOffset.UtcNow
        };

        var entity = await _context.Books.AddAsync(newBook);
        await _context.SaveChangesAsync();

        return entity.Entity;
    }

    public async Task<Book> UpdateAsync(Book modifiedJob)
    {
        _context.Entry(modifiedJob).State = EntityState.Modified;

        await _context.SaveChangesAsync();

        return modifiedJob;
    }
}