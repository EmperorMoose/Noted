using Noted.Shared.Models;
using System.ComponentModel.DataAnnotations;
//base class for note
public abstract class NoteBase
{
    //primary key
    public int? Id { get; set; } = null; //nullable id for new notes
    [Required]
    [MaxLength(100)]
    public string Title { get; set; } = string.Empty;
    [Required]
    [MaxLength(1000)]
    public string Content { get; set; } = string.Empty;

    public abstract Note ToNote();
}