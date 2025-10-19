using Noted.Application.Data;
using Noted.Shared.Models;
using Noted.Shared.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace Noted.Application;

public class NoteService : INoteService
{
    private readonly NotedDbContext _dbContext;

    public NoteService(NotedDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Note> CreateNote(Note note)
    {
        note.Id = default;
        _dbContext.Notes.Add(note);
        await _dbContext.SaveChangesAsync();
        return note;
    }

    public async Task<Note> GetNote(int? id)
    {
        var note = await _dbContext.Notes.FindAsync(id);

        if (note is null)
            throw new NoteNotFoundException();

        return note;
    }

    public async Task<Note> UpdateNote(Note newNote, string userId)
    {
        var oldNote = await CheckNoteExistsForUser(newNote.Id, userId);

        _dbContext.Notes.Update(newNote);
        await _dbContext.SaveChangesAsync();

        return newNote;
    }

    public async Task DeleteNote(int? id, string userId)
    {
        var note = await CheckNoteExistsForUser(id, userId);

        _dbContext.Notes.Remove(note);
        await _dbContext.SaveChangesAsync();

        return;
    }

    private async Task<Note> CheckNoteExistsForUser(int? id, string userId)
    {
        var note = await _dbContext.Notes
            .AsNoTracking()
            .FirstOrDefaultAsync(n => n.Id == id);

        if (note is null)
            throw new NoteNotFoundException();

        if (note.UserId != userId)
            throw new UnauthorizedNoteAccessException();

        return note;
    }
}
