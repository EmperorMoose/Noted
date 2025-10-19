using Microsoft.EntityFrameworkCore;
using Noted.Shared.Models;

namespace Noted.Application.Data;

public class NotedDbContext : DbContext
{
    public NotedDbContext(DbContextOptions<NotedDbContext> options) : base(options)
    {
    }

    public DbSet<Note> Notes => Set<Note>();
}