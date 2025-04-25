using Microsoft.EntityFrameworkCore;
using CommentsAPI.Models;

namespace CommentsAPI.Data
{
    public class CommentDBContext : DbContext
    {
        public CommentDBContext(DbContextOptions<CommentDBContext> options) : base(options) { }

        public DbSet<Comment> Comments => Set<Comment>();
    }
}