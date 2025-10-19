namespace Noted.Shared.Models;

//model to track notes
public class Note : NoteBase
{
    public string UserId { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; }

    public override Note ToNote()
    {
        return this;
    }
}