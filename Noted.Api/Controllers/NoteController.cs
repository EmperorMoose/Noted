using Microsoft.AspNetCore.Mvc;
using Noted.Application.Data;
using Noted.Shared.Models;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Noted.Application;
using Noted.Shared.Exceptions;

namespace Noted.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class NoteController : ControllerBase
{
    private readonly INoteService _noteService;

    public NoteController(INoteService noteService)
    {
        _noteService = noteService;
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> Create(NoteDto noteRequest)
    {
        Note note = noteRequest.ToNote();
        note.UserId = User.FindFirst("sub")?.Value ?? "unknown";
        
        try
        {
            await _noteService.CreateNote(note);

            return StatusCode(StatusCodes.Status201Created, note.Id);
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }

    [HttpGet("{id}")]
    [Authorize]
    public async Task<IActionResult> Get(int id)
    {
        try
        {
            var note = await _noteService.GetNote(id);
            return Ok(note);
        }
        catch (NoteNotFoundException)
        {
            return NotFound();
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }

    [Authorize]
    [HttpPut()]
    public async Task<IActionResult> Update(NoteDto noteRequest)
    {
        Note newNote = noteRequest.ToNote();
        //get the user id from the token
        var userId = User.FindFirst("sub")?.Value ?? "unknown";
        newNote.UserId = userId;

        try
        {
            if (newNote.Id is null)
            {
                return StatusCode(StatusCodes.Status400BadRequest);
            }

            var updatedNote = await _noteService.UpdateNote(newNote, userId);

            return StatusCode(StatusCodes.Status200OK);
        }
        catch (NoteNotFoundException)
        {
            return NotFound();
        }
        catch (UnauthorizedNoteAccessException)
        {
            return StatusCode(StatusCodes.Status403Forbidden);
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }

    [Authorize]
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var userId = User.FindFirst("sub")?.Value ?? "unknown";

        try
        {
            await _noteService.DeleteNote(id, userId);
            return StatusCode(StatusCodes.Status200OK);
        }
        catch (NoteNotFoundException)
        {
            return NotFound();
        }
        catch (UnauthorizedNoteAccessException)
        {
            return StatusCode(StatusCodes.Status403Forbidden);
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }
}