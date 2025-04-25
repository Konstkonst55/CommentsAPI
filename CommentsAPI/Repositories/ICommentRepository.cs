using CommentsAPI.Models;

namespace CommentsAPI.Repositories
{
    public interface ICommentRepository
    {
        IEnumerable<Comment> GetAll();
        Comment? GetById(int id);
        Comment Add(Comment comment);
        Comment? Update(int id, Comment updatedFields);
        bool Delete(int id);
    }
}
