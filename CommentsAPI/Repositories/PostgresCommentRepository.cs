using CommentsAPI.Models;
using CommentsAPI.Data;

namespace CommentsAPI.Repositories
{
    public class PostgresCommentRepository : ICommentRepository
    {
        private readonly CommentDBContext _context;

        public PostgresCommentRepository(CommentDBContext context)
        {
            _context = context;
        }

        public IEnumerable<Comment> GetAll() => _context.Comments.ToList();

        public Comment? GetById(int id) => _context.Comments.Find(id);

        public Comment Add(Comment comment)
        {
            _context.Comments.Add(comment);
            _context.SaveChanges();

            return comment;
        }

        public Comment? Update(int id, Comment updatedFields)
        {
            var existing = _context.Comments.Find(id);

            if (existing == null)
            {
                return null;
            }

            existing.PostId = updatedFields.PostId != 0 ? updatedFields.PostId : existing.PostId;
            existing.Name = updatedFields.Name ?? existing.Name;
            existing.Email = updatedFields.Email ?? existing.Email;
            existing.Body = updatedFields.Body ?? existing.Body;

            _context.SaveChanges();

            return existing;
        }

        public bool Delete(int id)
        {
            var comment = _context.Comments.Find(id);

            if (comment == null)
            {
                return false;
            }

            _context.Comments.Remove(comment);
            _context.SaveChanges();

            return true;
        }
    }
}