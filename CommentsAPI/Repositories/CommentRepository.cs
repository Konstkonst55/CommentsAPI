using CommentsAPI.Models;

namespace CommentsAPI.Repositories
{
    public class CommentRepository : ICommentRepository
    {
        private readonly Dictionary<int, Comment> _comments = [];
        private int _nextId = 1;

        public IEnumerable<Comment> GetAll() => _comments.Values;

        public Comment? GetById(int id) => _comments.TryGetValue(id, out var comment) ? comment : null;

        public Comment Add(Comment comment)
        {
            comment.Id = _nextId++;
            _comments[comment.Id] = comment;

            return comment;
        }

        public Comment? Update(int id, Comment updatedFields)
        {
            if (_comments.TryGetValue(id, out var existing))
            {
                existing.PostId = updatedFields.PostId != 0 ? updatedFields.PostId : existing.PostId;
                existing.Name = updatedFields.Name ?? existing.Name;
                existing.Email = updatedFields.Email ?? existing.Email;
                existing.Body = updatedFields.Body ?? existing.Body;

                return existing;
            }

            return null;
        }

        public bool Delete(int id) => _comments.Remove(id);
    }
}
