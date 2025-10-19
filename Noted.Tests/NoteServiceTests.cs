using Microsoft.EntityFrameworkCore;
using Noted.Application;
using Noted.Application.Data;
using Noted.Shared.Exceptions;
using Noted.Shared.Models;
using Xunit;
using FluentAssertions;

namespace Noted.Tests.Services;

public class NoteServiceTests : IDisposable
{
    private readonly NotedDbContext _context;
    private readonly NoteService _noteService;

    public NoteServiceTests()
    {
        var options = new DbContextOptionsBuilder<NotedDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        _context = new NotedDbContext(options);
        _noteService = new NoteService(_context);
    }

    [Fact]
    public async Task CreateNote_ShouldCreateNoteSuccessfully()
    {
        // Arrange
        var note = new Note
        {
            Title = "Test Note",
            Content = "Test Content",
            UserId = "user123"
        };

        // Act
        var result = await _noteService.CreateNote(note);

        // Assert
        result.Should().NotBeNull();
        result.Title.Should().Be("Test Note");
        result.Content.Should().Be("Test Content");
        result.UserId.Should().Be("user123");
        result.Id.Should().BeGreaterThan(0); // Changed: ID will be auto-generated
        
        // Verify it was saved to database
        var savedNote = await _context.Notes.FirstOrDefaultAsync(n => n.Title == "Test Note");
        savedNote.Should().NotBeNull();
    }

    [Fact]
    public async Task GetNote_WithValidId_ShouldReturnNote()
    {
        // Arrange
        var note = new Note
        {
            Id = 1,
            Title = "Test Note",
            Content = "Test Content",
            UserId = "user123"
        };

        _context.Notes.Add(note);
        await _context.SaveChangesAsync();

        // Act
        var result = await _noteService.GetNote(1);

        // Assert
        result.Should().NotBeNull();
        result.Id.Should().Be(1);
        result.Title.Should().Be("Test Note");
    }

    [Fact]
    public async Task GetNote_WithInvalidId_ShouldThrowNoteNotFoundException()
    {
        // Act & Assert
        await Assert.ThrowsAsync<NoteNotFoundException>(
            () => _noteService.GetNote(999));
    }

    [Fact]
    public async Task UpdateNote_WithValidUser_ShouldUpdateSuccessfully()
    {
        // Arrange - Create the note through the service
        var originalNote = new Note
        {
            Title = "Old Title",
            Content = "Old Content",
            UserId = "user123"
        };
        
        var createdNote = await _noteService.CreateNote(originalNote);

        _context.ChangeTracker.Clear();

        // Create the update note with the same ID
        var updatedNote = new Note
        {
            Id = createdNote.Id,
            Title = "New Title",
            Content = "New Content",
            UserId = "user123"
        };

        // Act
        var result = await _noteService.UpdateNote(updatedNote, "user123");

        // Assert
        result.Should().NotBeNull();
        result.Title.Should().Be("New Title");
        result.Content.Should().Be("New Content");
    }

    [Fact]
    public async Task UpdateNote_WithUnauthorizedUser_ShouldThrowUnauthorizedNoteAccessException()
    {
        // Arrange
        var existingNote = new Note
        {
            Id = 1,
            Title = "Test Title",
            Content = "Test Content",
            UserId = "user123"
        };

        _context.Notes.Add(existingNote);
        await _context.SaveChangesAsync();

        var updatedNote = new Note
        {
            Id = 1,
            Title = "New Title",
            Content = "New Content",
            UserId = "user123"
        };

        // Act & Assert
        await Assert.ThrowsAsync<UnauthorizedNoteAccessException>(
            () => _noteService.UpdateNote(updatedNote, "user456"));
    }

    [Fact]
    public async Task UpdateNote_WithNonExistentNote_ShouldThrowNoteNotFoundException()
    {
        // Arrange
        var updatedNote = new Note
        {
            Id = 999,
            Title = "New Title",
            Content = "New Content",
            UserId = "user123"
        };

        // Act & Assert
        await Assert.ThrowsAsync<NoteNotFoundException>(
            () => _noteService.UpdateNote(updatedNote, "user123"));
    }

    [Fact]
    public async Task DeleteNote_WithValidUser_ShouldDeleteSuccessfully()
    {
        // Arrange - Create the note through the service
        var note = new Note
        {
            Title = "Test Title",
            Content = "Test Content",
            UserId = "user123"
        };

        var createdNote = await _noteService.CreateNote(note);

        _context.ChangeTracker.Clear();

        // Act
        await _noteService.DeleteNote(createdNote.Id, "user123");

        // Assert
        var deletedNote = await _context.Notes.FirstOrDefaultAsync(n => n.Id == createdNote.Id);
        deletedNote.Should().BeNull();
    }

    [Fact]
    public async Task DeleteNote_WithUnauthorizedUser_ShouldThrowUnauthorizedNoteAccessException()
    {
        // Arrange
        var note = new Note
        {
            Id = 1,
            Title = "Test Title",
            Content = "Test Content",
            UserId = "user123"
        };

        _context.Notes.Add(note);
        await _context.SaveChangesAsync();

        // Act & Assert
        await Assert.ThrowsAsync<UnauthorizedNoteAccessException>(
            () => _noteService.DeleteNote(1, "user456"));
    }

    [Fact]
    public async Task DeleteNote_WithNonExistentNote_ShouldThrowNoteNotFoundException()
    {
        // Act & Assert
        await Assert.ThrowsAsync<NoteNotFoundException>(
            () => _noteService.DeleteNote(999, "user123"));
    }

    public void Dispose()
    {
        _context.Dispose();
    }
}