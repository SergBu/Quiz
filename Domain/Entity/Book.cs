using System.ComponentModel.DataAnnotations.Schema;
using SharpQuiz.Domain.Interfaces;

namespace SharpQuiz.Domain.Entity;

public class Book : IEntity<int>
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    
    public int Number { get; set; }
    
    public string Name { get; set; }  = null!;
    
    public string Type { get; set; }  = null!;
    public List<Chapter>? Chapters { get; set; }
}