using SharpQuiz.Domain.Entity;

namespace SharpQuiz.Database.Repositories.Interfaces;

public interface IBookRepository
{
    public Task<Book?> GetAsync(string bookId);
    public Task<Book> CreateAsync(string reportData, IEnumerable<string> TemplateCodes);
    public Task<Book> UpdateAsync(Book modifiedJob);
}