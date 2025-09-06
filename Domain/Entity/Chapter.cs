using System.ComponentModel.DataAnnotations.Schema;
using SharpQuiz.Domain.Interfaces;

namespace SharpQuiz.Domain.Entity;

public class Chapter : IEntity<int>
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    
    public int BookId { get; set; }
    
    public Book? Book { get; set; }

    public int Number { get; set; }
    
    public string Name { get; set; } = null!;
    
    public List<Clause>? Clauses { get; set; }
}