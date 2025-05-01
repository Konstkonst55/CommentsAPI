using Microsoft.EntityFrameworkCore;
using CommentsAPI.Models;
using Microsoft.Extensions.Logging.Abstractions;

namespace CommentsAPI.Data
{
    public class CommentDBContext : DbContext
    {
        public CommentDBContext(DbContextOptions<CommentDBContext> options) : base(options) { }

        public DbSet<Comment> Comments => Set<Comment>();
        public DbSet<LogEntry> Logs => Set<LogEntry>();
    }
}