using Noted.Shared.Models;

//dto for note requests
public class NoteDto : NoteBase
{
    //convert dto to note
    public override Note ToNote()
    {
        return new Note
        {
            Id = Id,
            Title = Title,
            Content = Content
        };
    }
}