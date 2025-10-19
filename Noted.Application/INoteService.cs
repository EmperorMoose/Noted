using Noted.Shared.Models;

namespace Noted.Application;

public interface INoteService
{
    Task<Note> CreateNote(Note note);
    Task<Note> GetNote(int? id);
    Task<Note> UpdateNote(Note newNote, string userId);
    Task DeleteNote(int? id, string userId);
}