using System.ComponentModel.DataAnnotations.Schema;
using SharpQuiz.Domain.Interfaces;

namespace SharpQuiz.Domain.Entity;

public class Clause : IEntity<int>
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    
    public int ChapterId { get; set; }
    
    public Chapter? Chapter { get; set; } 

    public int Number { get; set; }
    
    public string Question { get; set; } = null!;
    
    public string Answer { get; set; } = null!;
    
    public string? FullAnswer { get; set; }
}